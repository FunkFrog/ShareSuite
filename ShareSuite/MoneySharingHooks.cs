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
                    || !(bool) self.body
                    || !(bool) self.body.inventory)
                {
                    orig(self, info);
                    return;
                }

                var body = self.body;

                var preDamageMoney = self.body.master.money;

                orig(self, info);

                var postDamageMoney = self.body.master.money;

                if (body.inventory.GetItemCount(ItemIndex.GoldOnHit) <= 0) return;

                SharedMoneyValue -= (int) preDamageMoney - (int) postDamageMoney;

                foreach (var player in PlayerCharacterMasterController.instances)
                {
                    if (!(bool) player.master.GetBody() || player.master.GetBody() == body) continue;
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

                SharedMoneyValue += (int) body.master.money - (int) preDamageMoney;
            };
        }

        public static void ModifyGoldReward()
        {
            On.RoR2.DeathRewards.OnKilledServer += (orig, self, info) =>
            {
                orig(self, info);

                if (!ShareSuite.ModIsEnabled.Value) return;
                SharedMoneyValue += (int) self.goldReward;

                if (!ShareSuite.MoneyScalarEnabled.Value
                    || !NetworkServer.active) return;

                GiveAllScaledMoney(self.goldReward);
            };

            On.RoR2.BarrelInteraction.OnInteractionBegin += (orig, self, activator) =>
            {
                orig(self, activator);

                if (!ShareSuite.ModIsEnabled.Value) return;
                SharedMoneyValue += self.goldReward;

                if (!ShareSuite.MoneyScalarEnabled.Value
                    || !NetworkServer.active) return;

                GiveAllScaledMoney(self.goldReward);
            };
        }

        private static void GiveAllScaledMoney(float goldReward)
        {
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