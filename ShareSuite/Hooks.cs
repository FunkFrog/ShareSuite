using System;
using System.Linq;
using System.Reflection;
using MonoMod.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace ShareSuite
{
    public static class Hooks
    {
        private static int _bossItems = 1;
        // private static bool _sendPickup = true;

        private static readonly MethodInfo SendPickupMessage =
            typeof(GenericPickupController).GetMethod("SendPickupMessage",
                BindingFlags.NonPublic | BindingFlags.Static);

        public static void SplitTpMoney()
        {
            On.RoR2.TeleporterInteraction.OnInteractionBegin += (orig, self, activator) =>
            {
                if (ShareSuite.ModIsEnabled.Value && ShareSuite.OverrideBossLootScalingEnabled.Value)
                {
                    _bossItems = ShareSuite.BossLootCredit.Value;
                }
                else
                {
                    _bossItems = Run.instance.participatingPlayerCount;
                }

                if (self.isCharged && ShareSuite.MoneyIsShared.Value)
                {
                    foreach (var player in PlayerCharacterMasterController.instances)
                    {
                        player.master.money = (uint)
                            Mathf.FloorToInt(player.master.money / PlayerCharacterMasterController.instances.Count);
                    }
                }

                orig(self, activator);
            };
        }

        public static void BrittleCrownHook()
        {
            On.RoR2.HealthComponent.TakeDamage += (orig, self, info) =>
            {
                if (!NetworkServer.active) return;
                
                if (!ShareSuite.MoneyIsShared.Value
                    || !(bool) self.body
                    || !(bool) self.body.inventory)
                {
                    orig(self, info);
                    return;
                }

                var body = self.body;

                var preDamageMoney = self.body.master.money;

                orig(self, info);
 
                if (body.inventory.GetItemCount(ItemIndex.GoldOnHit) <= 0) return;
                foreach (var player in PlayerCharacterMasterController.instances)
                {
                    if (!(bool) player.master.GetBody() || player.master.GetBody() == body) continue;
                    player.master.money -= preDamageMoney - self.body.master.money;
                    EffectManager.instance.SimpleImpactEffect(Resources.Load<GameObject>(
                            "Prefabs/Effects/ImpactEffects/CoinImpact"),
                        player.master.GetBody().corePosition, Vector3.up, true);
                }
            };

            On.RoR2.GlobalEventManager.OnHitEnemy += (orig, self, info, victim) =>
            {
                if (!ShareSuite.MoneyIsShared.Value || !(bool) info.attacker ||
                    !(bool) info.attacker.GetComponent<CharacterBody>() ||
                    !(bool) info.attacker.GetComponent<CharacterBody>().master)
                {
                    orig(self, info, victim);
                    return;
                }

                var body = info.attacker.GetComponent<CharacterBody>();
                var preDamageMoney = body.master.money;

                orig(self, info, victim);

                if (!body.inventory || body.inventory.GetItemCount(ItemIndex.GoldOnHit) <= 0) return;
                
                foreach (var player in PlayerCharacterMasterController.instances)
                {
                    if (!(bool) player.master.GetBody() || player.master.GetBody() == body) continue;
                    player.master.money += body.master.money - preDamageMoney;
                }
            };
        }

        public static void ModifyGoldReward()
        {
            On.RoR2.DeathRewards.OnKilledServer += (orig, self, info) =>
            {
                orig(self, info);
                if (!ShareSuite.ModIsEnabled.Value
                    || !ShareSuite.MoneyScalarEnabled.Value
                    || !NetworkServer.active) return;

                GiveAllScaledMoney(self.goldReward);
            };

            On.RoR2.BarrelInteraction.OnInteractionBegin += (orig, self, activator) =>
            {
                orig(self, activator);
                if (!ShareSuite.ModIsEnabled.Value
                    || !ShareSuite.MoneyScalarEnabled.Value
                    || !NetworkServer.active) return;

                GiveAllScaledMoney(self.goldReward);
            };
        }

        /*public static void PickupFix()
        {
            On.RoR2.Chat.AddPickupMessage += (orig, body, pickupToken, pickupColor, pickupQuantity) =>
            {
                if (_sendPickup)
                    orig(body, pickupToken, pickupColor, pickupQuantity);
            };
        }*/

        private static void GiveAllScaledMoney(float goldReward)
        {
            foreach (var player in PlayerCharacterMasterController.instances.Select(p => p.master))
            {
                player.GiveMoney(
                    (uint) Mathf.Floor(goldReward * ShareSuite.MoneyScalar.Value - goldReward));
            }
        }

        public static void OverrideInteractablesScaling()
        {
            On.RoR2.SceneDirector.PlaceTeleporter += (orig, self) => //Replace 1 player values
            {
                orig(self);
                if (!ShareSuite.ModIsEnabled.Value) return;

                var defaultCredit = self.GetFieldValue<int>("interactableCredit");
                // Set interactables budget to 200 * config player count (normal calculation)
                if (ShareSuite.OverridePlayerScalingEnabled.Value)
                    self.SetFieldValue("interactableCredit", 200 * ShareSuite.InteractablesCredit.Value);
                SyncMoney();
            };
        }

        private static void SyncMoney()
        {
            if (!ShareSuite.MoneyIsShared.Value) return;
            foreach (var player in PlayerCharacterMasterController.instances)
            {
                player.master.money = NetworkUser.readOnlyInstancesList[0].master.money;
            }
        }

        public static void OverrideBossScaling()
        {
            
            IL.RoR2.BossGroup.DropRewards += il => // Replace boss drops
            {
                var c = new ILCursor(il).Goto(1);
                c.Remove();
                c.EmitDelegate<Func<Run, int>>(f => _bossItems);
            };
        }


        private static void SetEquipmentIndex(Inventory self, EquipmentIndex newEquipmentIndex, uint slot)
        {
            if (!NetworkServer.active) return;
            if (self.currentEquipmentIndex == newEquipmentIndex) return;
            var equipment = self.GetEquipment(0U);
            var charges = equipment.charges;
            if (equipment.equipmentIndex == EquipmentIndex.None) charges = 1;
            self.SetEquipment(new EquipmentState(newEquipmentIndex, equipment.chargeFinishTime, charges), slot);
        }
    
        public static void OnGrantEquipment()
        {
            On.RoR2.GenericPickupController.GrantEquipment += (orig, self, body, inventory) =>
            {
                var equip = self.pickupIndex.equipmentIndex;

                if (!ShareSuite.GetEquipmentBlackList().Contains((int) equip)
                    && NetworkServer.active
                    && IsValidEquipmentPickup(self.pickupIndex)
                    && IsMultiplayer()
                    && ShareSuite.ModIsEnabled.Value)
                    foreach (var player in PlayerCharacterMasterController.instances.Select(p => p.master)
                        .Where(p => p.alive || ShareSuite.DeadPlayersGetItems.Value))
                    {
                        SyncToolbotEquip(player, ref equip);
                        
                        // Sync Mul-T Equipment, but perform primary equipment pickup only for clients
                        if (player.inventory == inventory) continue;
                        
                        player.inventory.SetEquipmentIndex(equip);
                        self.NetworkpickupIndex = new PickupIndex(player.inventory.currentEquipmentIndex);
                        /*SendPickupMessage.Invoke(inventory.GetComponent<CharacterMaster>(),
                            new object[] {player, new PickupIndex(equip)});*/
                    }

                orig(self, body, inventory);
            };
        }

        private static void SyncToolbotEquip(CharacterMaster characterMaster, ref EquipmentIndex equip)
        {
            if (characterMaster.bodyPrefab.name != "ToolbotBody") return;
            SetEquipmentIndex(characterMaster.inventory, equip,
                (uint) (characterMaster.inventory.activeEquipmentSlot + 1) % 2);
        }

        public static void OnGrantItem()
        {
            On.RoR2.GenericPickupController.GrantItem += (orig, self, body, inventory) =>
            {
                // Item to share
                var item = self.pickupIndex.itemIndex;

                if (!ShareSuite.GetItemBlackList().Contains((int) item)
                    && NetworkServer.active
                    && IsValidItemPickup(self.pickupIndex)
                    && IsMultiplayer()
                    && ShareSuite.ModIsEnabled.Value)
                    foreach (var player in PlayerCharacterMasterController.instances.Select(p => p.master))
                    {
                        // Ensure character is not original player that picked up item
                        if (player.inventory == inventory) continue;
                        if (!player.alive && !ShareSuite.DeadPlayersGetItems.Value) continue;
                        player.inventory.GiveItem(item);
                        /*_sendPickup = false;
                        SendPickupMessage.Invoke(null, new object[] {player, self.pickupIndex});
                        _sendPickup = true;*/
                    }

                orig(self, body, inventory);
            };
        }

        public static void OnShopPurchase()
        {
            On.RoR2.PurchaseInteraction.OnInteractionBegin += (orig, self, activator) =>
            {
                if (!ShareSuite.ModIsEnabled.Value)
                {
                    orig(self, activator);
                    return;
                }
                
                // Return if you can't afford the item
                if (!self.CanBeAffordedByInteractor(activator)) return;
                
                var characterBody = activator.GetComponent<CharacterBody>();
                var inventory = characterBody.inventory;

                if (ShareSuite.MoneyIsShared.Value)
                {
                    //TODO add comments on what this does
                    switch (self.costType)
                    {
                        case CostTypeIndex.Money:
                        {
                            orig(self, activator);
                            foreach (var playerCharacterMasterController in PlayerCharacterMasterController.instances)
                            {
                                if (playerCharacterMasterController.master.alive &&
                                    playerCharacterMasterController.master.GetBody() != characterBody)
                                {
                                    playerCharacterMasterController.master.money -= (uint) self.cost;
                                }
                            }

                            return;
                        }

                        case CostTypeIndex.PercentHealth:
                        {
                            orig(self, activator);
                            var teamMaxHealth = 0;
                            foreach (var playerCharacterMasterController in PlayerCharacterMasterController.instances)
                            {
                                var charMaxHealth = playerCharacterMasterController.master.GetBody().maxHealth;
                                if (charMaxHealth > teamMaxHealth)
                                {
                                    teamMaxHealth = (int) charMaxHealth;
                                }
                            }

                            var purchaseInteraction = self.GetComponent<PurchaseInteraction>();
                            var shrineBloodBehavior = self.GetComponent<ShrineBloodBehavior>();
                            var amount = (uint) (teamMaxHealth * purchaseInteraction.cost / 100.0 *
                                                 shrineBloodBehavior.goldToPaidHpRatio);
                            
                            if (ShareSuite.MoneyScalarEnabled.Value) amount *= (uint) ShareSuite.MoneyScalar.Value;
                            var purchaseDiff =
                                amount - (uint) ((double) characterBody.maxHealth * purchaseInteraction.cost / 100.0 *
                                                 shrineBloodBehavior.goldToPaidHpRatio);

                            foreach (var playerCharacterMasterController in PlayerCharacterMasterController.instances)
                            {
                                if (!playerCharacterMasterController.master.alive) continue;
                                playerCharacterMasterController.master.GiveMoney(
                                    playerCharacterMasterController.master.GetBody() != characterBody
                                        ? amount
                                        : purchaseDiff);
                            }

                            return;
                        }
                    }
                }

                // If this is not a multi-player server or the fix is disabled, do the normal drop action
                if (!IsMultiplayer() || !ShareSuite.PrinterCauldronFixEnabled.Value)
                {
                    orig(self, activator);
                    return;
                }

                var shop = self.GetComponent<ShopTerminalBehavior>();

                // If the cost type is an item, give the user the item directly and send the pickup message
                if (self.costType == CostTypeIndex.WhiteItem
                    || self.costType == CostTypeIndex.GreenItem
                    || self.costType == CostTypeIndex.RedItem)
                {
                    var item = shop.CurrentPickupIndex().itemIndex;
                    inventory.GiveItem(item);
                    SendPickupMessage.Invoke(null,
                        new object[] {inventory.GetComponent<CharacterMaster>(), shop.CurrentPickupIndex()});
                }

                orig(self, activator);
            };
        }

        public static void OnPurchaseDrop()
        {
            On.RoR2.ShopTerminalBehavior.DropPickup += (orig, self) =>
            {
                if (!ShareSuite.ModIsEnabled.Value)
                {
                    orig(self);
                    return;
                }

                if (!NetworkServer.active) return;
                
                var costType = self.GetComponent<PurchaseInteraction>().costType;
                
                if (   !IsMultiplayer()
                    || !IsValidItemPickup(self.CurrentPickupIndex()) // item is not shared on pickup
                    && !ShareSuite.PrinterCauldronFixEnabled.Value // dupe fix is disabled
                    || self.itemTier == ItemTier.Lunar
                    || costType == CostTypeIndex.Money)
                {
                    orig(self);
                }
            };
        }

        /// <summary>
        /// This function is currently ineffective, but may be later extended to quickly set a validator
        /// on equipments to narrow them down to a set of ranges beyond just blacklisting.
        /// </summary>
        /// <param name="pickup">Takes a PickupIndex that's a valid equipment.</param>
        /// <returns>True if the given PickupIndex validates, otherwise false.</returns>
        private static bool IsValidEquipmentPickup(PickupIndex pickup)
        {
            var equip = pickup.equipmentIndex;
            return IsEquipment(equip) && ShareSuite.EquipmentShared.Value;
        }

        private static bool IsValidItemPickup(PickupIndex pickup)
        {
            var item = pickup.itemIndex;
            return IsWhiteItem(item) && ShareSuite.WhiteItemsShared.Value
                   || IsGreenItem(item) && ShareSuite.GreenItemsShared.Value
                   || IsRedItem(item) && ShareSuite.RedItemsShared.Value
                   || pickup.IsLunar() && ShareSuite.LunarItemsShared.Value
                   || IsBossItem(item) && ShareSuite.BossItemsShared.Value;
        }

        private static bool IsMultiplayer()
        {
            // Check if there are more then 1 players in the lobby
            return PlayerCharacterMasterController.instances.Count > 1;
        }

        public static bool IsWhiteItem(ItemIndex index)
        {
            return ItemCatalog.tier1ItemList.Contains(index);
        }

        public static bool IsGreenItem(ItemIndex index)
        {
            return ItemCatalog.tier2ItemList.Contains(index);
        }

        public static bool IsRedItem(ItemIndex index)
        {
            return ItemCatalog.tier3ItemList.Contains(index);
        }

        public static bool IsEquipment(EquipmentIndex index)
        {
            return EquipmentCatalog.allEquipment.Contains(index);
        }

        public static bool IsBossItem(ItemIndex index)
        {
            return index == ItemIndex.Knurl
                || index == ItemIndex.SprintWisp
                || index == ItemIndex.TitanGoldDuringTP
                || index == ItemIndex.BeetleGland;
        }

        public static bool IsQueensGland(ItemIndex index)
        {
            return index == ItemIndex.BeetleGland;
        }
    }
}