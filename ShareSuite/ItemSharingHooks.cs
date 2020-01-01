using System.Linq;
using System.Reflection;
using RoR2;
using UnityEngine.Networking;

namespace ShareSuite
{
    public static class ItemSharingHooks
    {
        public static void UnHook()
        {
            On.RoR2.PurchaseInteraction.OnInteractionBegin -= OnShopPurchase;
            On.RoR2.ShopTerminalBehavior.DropPickup -= OnPurchaseDrop;
            On.RoR2.GenericPickupController.GrantItem -= OnGrantItem;
        }
        public static void Hook()
        {
            On.RoR2.PurchaseInteraction.OnInteractionBegin += OnShopPurchase;
            On.RoR2.ShopTerminalBehavior.DropPickup += OnPurchaseDrop;
            On.RoR2.GenericPickupController.GrantItem += OnGrantItem;
        }

        private static void OnGrantItem(On.RoR2.GenericPickupController.orig_GrantItem orig, GenericPickupController self, CharacterBody body, Inventory inventory)
        {
            var item = PickupCatalog.GetPickupDef(self.pickupIndex).itemIndex;

            if (!ShareSuite.GetItemBlackList().Contains((int)item)
                && NetworkServer.active
                && IsValidItemPickup(self.pickupIndex)
                && GeneralHooks.IsMultiplayer())
                foreach (var player in PlayerCharacterMasterController.instances.Select(p => p.master))
                {
                    // Ensure character is not original player that picked up item
                    if (player.inventory == inventory) continue;

                    // Do not reward dead players if not required
                    if (!player.alive && !ShareSuite.DeadPlayersGetItems.Value) continue;

                    player.inventory.GiveItem(item);
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
                SendPickupMessage.Invoke(null,
                    new object[] { inventory.GetComponent<CharacterMaster>(), shop.CurrentPickupIndex() });
            }

            #endregion Cauldronfix

            orig(self, activator);
        }

        private static readonly MethodInfo SendPickupMessage =
            typeof(GenericPickupController).GetMethod("SendPickupMessage",
                BindingFlags.NonPublic | BindingFlags.Static);


        private static bool IsValidItemPickup(PickupIndex pickup)
        {
            var pickupdef = PickupCatalog.GetPickupDef(pickup);
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
                default:
                    return false;
            }
        }
    }
}