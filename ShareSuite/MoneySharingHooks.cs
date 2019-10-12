using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace ShareSuite
{
    public static class MoneySharingHooks
    {
        public static int SharedMoneyValue;

        public static void BrittleCrownHook()
        {
            On.RoR2.HealthComponent.TakeDamage += (orig, self, info) =>
            {
                if (!NetworkServer.active) return;

                if (!ShareSuite.MoneyIsShared.Value
                    || !(bool)self.body
                    || !(bool)self.body.inventory
                    || !ShareSuite.ModIsEnabled.Value)
                {
                    orig(self, info);
                    return;
                }

                #region Sharedmoney
                // The idea here is that we track amount of money pre and post function evaluation.
                // We can subsequently apply the difference to the shared pool.
                var body = self.body;

                var preDamageMoney = self.body.master.money;

                orig(self, info);

                var postDamageMoney = self.body.master.money;

                // Ignore all of this if we do not actually have the item
                if (body.inventory.GetItemCount(ItemIndex.GoldOnHit) <= 0) return;

                // Apply the calculation to the shared money pool
                SharedMoneyValue += (int)postDamageMoney - (int)preDamageMoney;

                // Add impact effect
                foreach (var player in PlayerCharacterMasterController.instances)
                {
                    if (!(bool) player.master.GetBody() || player.master.GetBody() == body) continue;
                    EffectManager.instance.SimpleImpactEffect(Resources.Load<GameObject>(
                            "Prefabs/Effects/ImpactEffects/CoinImpact"),
                        player.master.GetBody().corePosition, Vector3.up, true);
                }
                #endregion
            };

            On.RoR2.GlobalEventManager.OnHitEnemy += (orig, self, info, victim) =>
            {
                if (!ShareSuite.MoneyIsShared.Value
                    || !(bool) info.attacker
                    || !(bool) info.attacker.GetComponent<CharacterBody>()
                    || !(bool) info.attacker.GetComponent<CharacterBody>().master
                    || !ShareSuite.ModIsEnabled.Value)
                {
                    orig(self, info, victim);
                    return;
                }

                #region Sharedmoney
                // The idea here is that we track amount of money pre and post function evaluation.
                // We can subsequently apply the difference to the shared pool.
                var body = info.attacker.GetComponent<CharacterBody>();

                var preDamageMoney = body.master.money;

                orig(self, info, victim);

                var postDamageMoney = body.master.money;

                // Ignore all of this if we do not actually have the item
                if (!body.inventory || body.inventory.GetItemCount(ItemIndex.GoldOnHit) <= 0) return;

                // Apply the calculation to the shared money pool
                SharedMoneyValue += (int) postDamageMoney - (int) preDamageMoney;
                #endregion
            };
        }

        public static void ModifyGoldReward()
        {
            On.RoR2.DeathRewards.OnKilledServer += (orig, self, info) =>
            {
                orig(self, info);

                if (!ShareSuite.ModIsEnabled.Value) return;

                #region Sharedmoney
                // Collect reward from kill and put it into shared pool
                SharedMoneyValue += (int) self.goldReward;

                if (!ShareSuite.MoneyScalarEnabled.Value
                    || !NetworkServer.active) return;

                GiveAllScaledMoney(self.goldReward);
                #endregion
            };

            On.RoR2.BarrelInteraction.OnInteractionBegin += (orig, self, activator) =>
            {
                orig(self, activator);

                #region Sharedmoney
                // Collect reward from barrel and put it into shared pool
                if (!ShareSuite.ModIsEnabled.Value) return;
                SharedMoneyValue += self.goldReward;

                if (!ShareSuite.MoneyScalarEnabled.Value
                    || !NetworkServer.active) return;

                GiveAllScaledMoney(self.goldReward);
                #endregion
            };
        }

        private static void GiveAllScaledMoney(float goldReward)
        {
            //Apply gold rewards to shared money pool
            SharedMoneyValue += (int) Mathf.Floor(goldReward * ShareSuite.MoneyScalar.Value - goldReward);
        }

        public static void AdjustTpMoney()
        {
            var players = PlayerCharacterMasterController.instances.Count;
            SharedMoneyValue /= players;
            foreach (var player in PlayerCharacterMasterController.instances)
            {
                player.master.money = (uint)
                    Mathf.FloorToInt(player.master.money / players);
            }
        }
    }
}