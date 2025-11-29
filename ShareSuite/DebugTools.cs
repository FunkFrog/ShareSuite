#if DEBUG
using RoR2;
using UnityEngine;


namespace ShareSuite
{
    internal class DebugTools
    {
        internal static void Hook()
        {
            On.RoR2.HealthComponent.TakeDamage += DontTakeDamage;
            On.RoR2.SceneDirector.PopulateScene += OverridePopulateScene;
        }

        internal static void UnHook()
        {
            On.RoR2.HealthComponent.TakeDamage -= DontTakeDamage;
            On.RoR2.SceneDirector.PopulateScene -= OverridePopulateScene;
        }

        private static void DontTakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig,
            HealthComponent self, DamageInfo damageInfo)
        {
            if (ShareSuite.GodModeEnabled.Value && self.body.isPlayerControlled)
            {
                return;
            }
            orig(self, damageInfo);
        }

        private static void OverridePopulateScene(On.RoR2.SceneDirector.orig_PopulateScene orig,
    SceneDirector self)
        {
            Debug.Log($"Populator has Credit value: {self.interactableCredit}");
            orig(self);
        }
    }
}

#endif