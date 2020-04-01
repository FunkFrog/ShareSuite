using System;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

namespace ShareSuite
{
    public static class ItemSharingHooks
    {
        public static void UnHook()
        {
            On.RoR2.PurchaseInteraction.OnInteractionBegin -= OnShopPurchase;
            On.RoR2.ShopTerminalBehavior.DropPickup -= OnPurchaseDrop;
            On.RoR2.GenericPickupController.GrantItem -= OnGrantItem;
            On.RoR2.ScavBackpackBehavior.RollItem -= OnScavengerDrop;
            IL.RoR2.ArenaMissionController.EndRound -= ArenaDropEnable;
        }
        public static void Hook()
        {
            On.RoR2.PurchaseInteraction.OnInteractionBegin += OnShopPurchase;
            On.RoR2.ShopTerminalBehavior.DropPickup += OnPurchaseDrop;
            On.RoR2.GenericPickupController.GrantItem += OnGrantItem;
            On.RoR2.ScavBackpackBehavior.RollItem += OnScavengerDrop;
            IL.RoR2.ArenaMissionController.EndRound += ArenaDropEnable;
        }

        private static void OnGrantItem(On.RoR2.GenericPickupController.orig_GrantItem orig, GenericPickupController self, CharacterBody body, Inventory inventory)
        {
            var item = PickupCatalog.GetPickupDef(self.pickupIndex);
            var itemDef = ItemCatalog.GetItemDef(item.itemIndex);


              if ((ShareSuite.RandomizeSharedPickups.Value || !ShareSuite.GetItemBlackList().Contains((int)item.itemIndex))
                && NetworkServer.active
                && IsValidItemPickup(self.pickupIndex)
                && GeneralHooks.IsMultiplayer())
                foreach (var player in PlayerCharacterMasterController.instances.Select(p => p.master))
                {
                    // Ensure character is not original player that picked up item
                    if (player.inventory == inventory) continue;

                    // Ensure body exists

                    if (!player.hasBody) return;
                    
                    // Do not reward dead players if not required
                    if (!player.GetBody().healthComponent.alive && !ShareSuite.DeadPlayersGetItems.Value) continue;

                    if (ShareSuite.RandomizeSharedPickups.Value)
                    {
                        var itemIsBlacklisted = true;
                        var giveItem = GetRandomItemOfTier(itemDef.tier, item.pickupIndex);
                        while (itemIsBlacklisted)
                        {
                            if (ShareSuite.GetItemBlackList().Contains((int) giveItem.itemIndex))
                            {
                                giveItem = GetRandomItemOfTier(itemDef.tier, item.pickupIndex);
                            }
                            else
                            {
                                itemIsBlacklisted = false;
                            }
                        }
                        player.inventory.GiveItem(giveItem.itemIndex);
                        // Alternative: Only show pickup text for yourself
                        // var givePickupDef = PickupCatalog.GetPickupDef(givePickupIndex);
                        // Chat.AddPickupMessage(body, givePickupDef.nameToken, givePickupDef.baseColor, 1);
                        SendPickupMessage(player, giveItem);
                    }
                    // Otherwise give everyone the same item
                    else
                    {
                        player.inventory.GiveItem(item.itemIndex);
                    }
                }

            orig(self, body, inventory);
        }

        private static void OnPurchaseDrop(On.RoR2.ShopTerminalBehavior.orig_DropPickup orig, ShopTerminalBehavior self)
        {
            if (!NetworkServer.active)
            {
                orig(self);
                return;
            }

            var costType = self.GetComponent<PurchaseInteraction>().costType;

            if (!GeneralHooks.IsMultiplayer() // is not multiplayer
                || !IsValidItemPickup(self.CurrentPickupIndex()) // item is not shared
                || !ShareSuite.PrinterCauldronFixEnabled.Value // dupe fix isn't enabled
                || self.itemTier == ItemTier.Lunar
                || costType == CostTypeIndex.Money)
            {
                orig(self);
            }
        }

        private static void OnShopPurchase(On.RoR2.PurchaseInteraction.orig_OnInteractionBegin orig, PurchaseInteraction self, Interactor activator)
        {
            if (!self.CanBeAffordedByInteractor(activator)) return;

            var characterBody = activator.GetComponent<CharacterBody>();
            var inventory = characterBody.inventory;

            #region Cauldronfix

            // If this is not a multi-player server or the fix is disabled, do the normal drop action
            if (!GeneralHooks.IsMultiplayer() || !ShareSuite.PrinterCauldronFixEnabled.Value)
            {
                orig(self, activator);
                return;
            }

            var shop = self.GetComponent<ShopTerminalBehavior>();

            // If the cost type is an item, give the user the item directly and send the pickup message
            if (self.costType == CostTypeIndex.WhiteItem
                || self.costType == CostTypeIndex.GreenItem
                || self.costType == CostTypeIndex.RedItem
                || self.costType == CostTypeIndex.BossItem
                || self.costType == CostTypeIndex.LunarItemOrEquipment)
            {
                var item = PickupCatalog.GetPickupDef(shop.CurrentPickupIndex()).itemIndex;
                inventory.GiveItem(item);
                SendPickupMessage(inventory.GetComponent<CharacterMaster>(), shop.CurrentPickupIndex());
            }

            #endregion Cauldronfix

            orig(self, activator);
        }

        private static void OnScavengerDrop(On.RoR2.ScavBackpackBehavior.orig_RollItem orig, ScavBackpackBehavior self)
        {
            //TODO Doesn't work. Current intended effect is to divide the rolled amount of drops by the amount of players, with a minimum of 2 items dropped from the pack
            
            /*double defaultDrops = EntityStates.ScavBackpack.Opening.maxItemDropCount;
            var adjustedDrops = (int) Math.Floor(defaultDrops / Run.instance.participatingPlayerCount);
            EntityStates.ScavBackpack.Opening.maxItemDropCount = adjustedDrops >= 2 ? adjustedDrops : 2;*/
            orig(self);
        }
        
        //Void Fields item fix
        public static void ArenaDropEnable(ILContext il)
        {
            if (!ShareSuite.OverrideVoidFieldLootScalingEnabled.Value) return;
            var cursor = new ILCursor(il);

            cursor.GotoNext(
                x => x.MatchLdloc(1),
                x => x.MatchStloc(out _),
                x => x.MatchLdcR4(out _),
                x => x.MatchLdloc(out _)
            );
            cursor.Index++;
            cursor.EmitDelegate<Func<int, int>>(i => ShareSuite.VoidFieldLootCredit.Value);
        }

        private delegate void SendPickupMessageDelegate(CharacterMaster master, PickupIndex pickupIndex);

        private static readonly SendPickupMessageDelegate SendPickupMessage =
            (SendPickupMessageDelegate)Delegate.CreateDelegate(typeof(SendPickupMessageDelegate), typeof(GenericPickupController).GetMethod("SendPickupMessage", BindingFlags.NonPublic | BindingFlags.Static));

        private static bool IsValidItemPickup(PickupIndex pickup)
        {
            var pickupdef = PickupCatalog.GetPickupDef(pickup);
            if (pickupdef.itemIndex != ItemIndex.None)
            {
                var itemdef = ItemCatalog.GetItemDef(pickupdef.itemIndex);
                switch (itemdef.tier)
                {
                    case ItemTier.Tier1:
                        return ShareSuite.WhiteItemsShared.Value;
                    case ItemTier.Tier2:
                        return ShareSuite.GreenItemsShared.Value;
                    case ItemTier.Tier3:
                        return ShareSuite.RedItemsShared.Value;
                    case ItemTier.Lunar:
                        return ShareSuite.LunarItemsShared.Value;
                    case ItemTier.Boss:
                        return ShareSuite.BossItemsShared.Value;
                    case ItemTier.NoTier:
                        break;
                    default:
                        return false;
                }
            }
            if (pickupdef.equipmentIndex != EquipmentIndex.None)
            {
                // var equipdef = EquipmentCatalog.GetEquipmentDef(pickupdef.equipmentIndex);
                // Optional further checks ...
                return false;
            }
            return false;
        }

        private static PickupIndex GetRandomItemOfTier(ItemTier tier, PickupIndex orDefault)
        {
            switch (tier)
            {
                case ItemTier.Tier1:
                    return PickRandomOf(Run.instance.availableTier1DropList);
                case ItemTier.Tier2:
                    return PickRandomOf(Run.instance.availableTier2DropList);
                case ItemTier.Tier3:
                    return PickRandomOf(Run.instance.availableTier3DropList);
                case ItemTier.Lunar:
                    return PickRandomOf(Run.instance.availableLunarDropList);
                case ItemTier.Boss:
                    return orDefault; // no boss item list, and also probably better anyway
                default:
                    return orDefault;
            }
        }

        private static T PickRandomOf<T>(IList<T> collection) => collection[Random.Range(0, collection.Count)];
    }
}