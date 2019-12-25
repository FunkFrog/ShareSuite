using System;
using R2API;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;

namespace ShareSuite
{
    public static class GeneralHooks
    {
        public static int BossItems = 1;
        public static List<string> NoInteractibleOverrideScenes = new List<string>{ "MAP_BAZAAR_TITLE" };

        internal static void Hook()
        {
            On.RoR2.BossGroup.DropRewards += BossGroup_DropRewards;
            On.RoR2.SceneDirector.PlaceTeleporter += InteractibleCreditOverride;
            On.RoR2.TeleporterInteraction.OnInteractionBegin += overrideBoosLootScaling;
        }

        private static void BossGroup_DropRewards(On.RoR2.BossGroup.orig_DropRewards orig, BossGroup self)
        {
            ItemDropAPI.BossDropParticipatingPlayerCount = BossItems;
            orig(self);
        }

        internal static void UnHook()
        {
            On.RoR2.BossGroup.DropRewards -= BossGroup_DropRewards;
            On.RoR2.SceneDirector.PlaceTeleporter -= InteractibleCreditOverride;
            On.RoR2.TeleporterInteraction.OnInteractionBegin -= overrideBoosLootScaling;
        }

        /// <summary>
        /// // Helper function for Bossloot
        /// </summary>
        private static void overrideBoosLootScaling(On.RoR2.TeleporterInteraction.orig_OnInteractionBegin orig, TeleporterInteraction self, Interactor activator)
        {
            if (ShareSuite.OverrideBossLootScalingEnabled.Value)
                BossItems = ShareSuite.BossLootCredit.Value;
            else
                BossItems = Run.instance.participatingPlayerCount;

            orig(self, activator);
            }


        public static bool IsMultiplayer()
        {
            // Check whether the quantity of players in the lobby exceeds one.
            return PlayerCharacterMasterController.instances.Count > 1;
        }

        private static void InteractibleCreditOverride(On.RoR2.SceneDirector.orig_PlaceTeleporter orig, SceneDirector self)
        {
            orig(self);

            #region InteractablesCredit

            // This is the standard amount of interactablesCredit we work with.
            // Prior to the interactablesCredit overhaul this was the standard value for all runs.
            var interactableCredit = 200;

            var stageInfo = SceneInfo.instance.GetComponent<ClassicStageInfo>();

            if (stageInfo)
            {
                // Overwrite our base value with the actual amount of director credits.
                interactableCredit = stageInfo.sceneDirectorInteractibleCredits;

                // We require playercount for several of the following computations. We don't want this to break with
                // those crazy 'mega party mods', thus we clamp this value.
                var clampPlayerCount = System.Math.Min(Run.instance.participatingPlayerCount, 8);

                // The flat creditModifier slightly adjust interactables based on the amount of players.
                // We do not want to reduce the amount of interactables too much for very high amounts of players (to support multiplayer mods).
                var creditModifier = (float)(0.95 + clampPlayerCount * 0.05);

                // In addition to our flat modifier, we additionally introduce a stage modifier.
                // This reduces player strength early game (as having more bodies gives a flat power increase early game).
                creditModifier *= (float)System.Math.Max(
                                        1.0 + 0.1 * System.Math.Min(
                                            Run.instance.participatingPlayerCount * 2 - Run.instance.stageClearCount - 2,
                                            3)
                                        , 1.0);

                // We must apply the transformation to interactableCredit otherwise bonusIntractableCreditObject will be overwritten.
                interactableCredit = (int)(interactableCredit / creditModifier);

                // Fetch the amount of bonus interactables we may play with. We have to do this after our first math block,
                // as we do not want to divide bonuscredits twice.
                if (stageInfo.bonusInteractibleCreditObjects != null)
                {
                    foreach (var bonusInteractableCreditObject in stageInfo.bonusInteractibleCreditObjects)
                    {
                        if (bonusInteractableCreditObject.objectThatGrantsPointsIfEnabled.activeSelf)
                        {
                            interactableCredit += bonusInteractableCreditObject.points / clampPlayerCount;
                        }
                    }
                }
            }

            // Set interactables budget to interactableCredit * config player count.
            if (ShareSuite.OverridePlayerScalingEnabled.Value && (!SceneInfo.instance || !NoInteractibleOverrideScenes.Contains(SceneInfo.instance.sceneDef.nameToken) ))
                self.SetFieldValue<int>("interactableCredit", (int)(interactableCredit * ShareSuite.InteractablesCredit.Value));

            #endregion
        }
    }
}
