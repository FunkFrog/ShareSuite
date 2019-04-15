using Harmony;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace ShareSuite
{
    public static class Hooks
    {
        public static void DisableInteractablesScaling()
        {
            if (ShareSuite.WrapDisablePlayerScalingEnabled)
                On.RoR2.SceneDirector.PlaceTeleporter += (orig, self) => //Replace 1 player values
                {
                    if (!NetworkServer.active) return;
                    // Set interactables budget to 200 * config player count (normal calculation)
                    AccessTools.Field(AccessTools.TypeByName("RoR2.SceneDirector"), "interactableCredit")
                        .SetValue(self, 200 * ShareSuite.WrapInteractablesCredit);
                    orig(self);
                };

            if (ShareSuite.WrapDisableBossLootScalingEnabled)
                IL.RoR2.BossGroup.OnCharacterDeathCallback += il => // Replace boss drops
                {
                    if (!NetworkServer.active) return;
                    // Remove line where boss loot amount is specified and replace it with WrapBossLootCredit
                    var c = new ILCursor(il).Goto(99); //146?
                    c.Remove();
                    c.Emit(OpCodes.Ldc_I4, ShareSuite.WrapBossLootCredit);
                };
        }

        public static void OnGrantItem()
        {
            On.RoR2.GenericPickupController.GrantItem += (orig, self, body, inventory) =>
            {
                if (!NetworkServer.active) return;
                // Give original player the item
                orig(self, body, inventory);

                // Do nothing else if single-player or 
                if (!IsMultiplayer()) return;
                if (!NetworkServer.active) return;

                // Item to share
                var item = self.pickupIndex.itemIndex;

                // Iterate over all player characters in game
                if (IsValidPickup(self.pickupIndex))
                    foreach (var playerCharacterMasterController in PlayerCharacterMasterController.instances)
                    {
                        // Ensure character is not original player that picked up item
                        if (playerCharacterMasterController.master.GetBody().Equals(body)) continue;
                        // Ensure character is alive
                        if (!playerCharacterMasterController.master.alive) continue;

                        // Give character the item
                        playerCharacterMasterController.master.inventory.GiveItem(item);
                    }
            };
        }

        public static void ModifyGoldReward()
        {
            if (ShareSuite.WrapMoneyIsShared)
            {
                On.RoR2.DeathRewards.OnKilled += (orig, self, info) =>
                {
                    if (!NetworkServer.active) return;
                    // extraGold is the normal reward * player count - normal reward (so 4 players would get 4x normal gold)
                    var extraGold = self.goldReward * PlayerCharacterMasterController.instances.Count - self.goldReward;
                    foreach (var playerCharacterMasterController in PlayerCharacterMasterController.instances)
                    {
                        // Add money to players w/ scalar
                        playerCharacterMasterController.master.GiveMoney(
                            (uint) Mathf.Floor(extraGold * ShareSuite.MoneyScalar));
                    }

                    // give the normal amount of money and perform other onkill actions
                    orig(self, info);
                };
            }
        }

        public static void OnShopPurchase()
        {
            On.RoR2.PurchaseInteraction.OnInteractionBegin += (orig, self, activator) =>
            {
                if (!NetworkServer.active) return;
                // Return if you can't afford the item
                if (!self.CanBeAffordedByInteractor(activator)) return;

                var characterBody = activator.GetComponent<CharacterBody>();
                var inventory = characterBody.inventory;

                if (ShareSuite.WrapMoneyIsShared)
                {
                    //TODO add comments on what this does
                    switch (self.costType)
                    {
                        case CostType.Money:
                        {
                            orig(self, activator);
                            foreach (var playerCharacterMasterController in PlayerCharacterMasterController.instances)
                            {
                                if (playerCharacterMasterController.master.alive &&
                                    playerCharacterMasterController.master.GetBody() != characterBody)
                                {
                                    playerCharacterMasterController.master.money -= (uint) self.cost;
                                    Debug.Log("Gave " + playerCharacterMasterController.master.GetBody()
                                                  .GetDisplayName() + " money");
                                }
                            }

                            return;
                        }

                        case CostType.PercentHealth:
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
                            var amount = (uint) (teamMaxHealth * purchaseInteraction.cost / 100.0 * 0.5f *
                                                 ShareSuite.MoneyScalar);
                            var purchaseDiff =
                                amount - (uint) ((double) characterBody.maxHealth * purchaseInteraction.cost / 100.0 *
                                                 0.5f);

                            foreach (var playerCharacterMasterController in PlayerCharacterMasterController.instances)
                            {
                                if (playerCharacterMasterController.master.alive)
                                {
                                    if (playerCharacterMasterController.master.GetBody() != characterBody)
                                    {
                                        playerCharacterMasterController.master.GiveMoney(amount);
                                        Debug.Log("Gave " + playerCharacterMasterController.master.GetBody()
                                                      .GetDisplayName() + " money");
                                    }
                                    else
                                    {
                                        playerCharacterMasterController.master.GiveMoney(purchaseDiff);
                                    }
                                }
                            }

                            return;
                        }
                    }
                }

                // If this is not a multi-player server or the fix is disabled, do the normal drop action
                if (!IsMultiplayer() || !ShareSuite.WrapPrinterCauldronFixEnabled)
                {
                    orig(self, activator);
                    return;
                }

                var shop = self.GetComponent<ShopTerminalBehavior>();

                // If the cost type is an item, give the user the item directly and send the pickup message
                if (self.costType == CostType.WhiteItem
                    || self.costType == CostType.GreenItem
                    || self.costType == CostType.RedItem)
                {
                    var item = shop.CurrentPickupIndex().itemIndex;
                    inventory.GiveItem(item);
                    Chat.AddPickupMessage(characterBody, ItemCatalog.GetItemDef(item).nameToken, GetItemColor(item),
                        (uint) characterBody.inventory.GetItemCount(item));
                }

                orig(self, activator);
            };
        }

        public static void OnPurchaseDrop()
        {
            On.RoR2.ShopTerminalBehavior.DropPickup += (orig, self) =>
            {
                if (!NetworkServer.active) return;
                var costType = self.GetComponent<PurchaseInteraction>().costType;
                Debug.Log("Cost type: " + costType);
                // If this is a multi-player lobby and the fix is enabled and it's not a lunar item, don't drop an item
                if (!IsMultiplayer()
                    || !IsValidPickup(self.CurrentPickupIndex())
                    || !ShareSuite.WrapPrinterCauldronFixEnabled
                    || self.itemTier == ItemTier.Lunar
                    || costType == CostType.Money)
                {
                    // Else drop the item
                    orig(self);
                }
            };
        }

        private static bool IsValidPickup(PickupIndex pickup)
        {
            var item = pickup.itemIndex;
            return IsWhiteItem(item) && ShareSuite.WrapWhiteItemsShared
                   || IsGreenItem(item) && ShareSuite.WrapGreenItemsShared
                   || IsRedItem(item) && ShareSuite.WrapRedItemsShared
                   || IsLunarItem(item) && ShareSuite.WrapLunarItemsShared
                   || IsBossItem(item) && ShareSuite.WrapBossItemsShared
                   || IsQueensGland(item) && ShareSuite.WrapQueensGlandsShared;
        }

        private static bool IsMultiplayer()
        {
            // Check if there are more then 1 players in the lobby
            return PlayerCharacterMasterController.instances.Count > 1;
        }

        public static Color32 GetItemColor(ItemIndex index)
        {
            if (IsWhiteItem(index))
                return ColorCatalog.GetColor(ColorCatalog.ColorIndex.Tier1Item);
            if (IsGreenItem(index))
                return ColorCatalog.GetColor(ColorCatalog.ColorIndex.Tier2Item);
            if (IsRedItem(index))
                return ColorCatalog.GetColor(ColorCatalog.ColorIndex.Tier3Item);
            if (IsLunarItem(index))
                return ColorCatalog.GetColor(ColorCatalog.ColorIndex.LunarItem);
            if (IsBossItem(index))
                return ColorCatalog.GetColor(ColorCatalog.ColorIndex.BossItem);
            return Color.white;
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

        public static bool IsBossItem(ItemIndex index)
        {
            return index == ItemIndex.Knurl;
        }

        public static bool IsQueensGland(ItemIndex index)
        {
            return index == ItemIndex.BeetleGland;
        }

        public static bool IsLunarItem(ItemIndex index)
        {
            return ItemCatalog.lunarItemList.Contains(index);
        }
    }
}