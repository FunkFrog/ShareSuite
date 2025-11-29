using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShareSuite
{
    public static class GeneralHooks
    {
        private static int _sacrificeOffset = 1;
        private static int _bossItems = 1;

        private static List<string> NoInteractibleOverrideScenes = new List<string>
        {
            "MAP_BAZAAR_TITLE",
            "MAP_ARENA_TITLE", "MAP_LIMBO_TITLE", "MAP_MYSTERYSPACE_TITLE"
        };

        internal static void Hook()
        {
            On.RoR2.BossGroup.DropRewards += BossGroup_DropRewards;
            On.RoR2.SceneDirector.PlaceTeleporter += InteractibleCreditOverride;
            On.RoR2.TeleporterInteraction.OnInteractionBegin += OverrideBossLootScaling;
            On.RoR2.Artifacts.SacrificeArtifactManager.OnPrePopulateSceneServer += SetSacrificeOffset;
            On.RoR2.Util.GetExpAdjustedDropChancePercent += GetExpAdjustedDropChancePercent;
        }

        internal static void UnHook()
        {
            On.RoR2.BossGroup.DropRewards -= BossGroup_DropRewards;
            On.RoR2.SceneDirector.PlaceTeleporter -= InteractibleCreditOverride;
            On.RoR2.TeleporterInteraction.OnInteractionBegin -= OverrideBossLootScaling;
            On.RoR2.Artifacts.SacrificeArtifactManager.OnPrePopulateSceneServer -= SetSacrificeOffset;
            On.RoR2.Util.GetExpAdjustedDropChancePercent -= GetExpAdjustedDropChancePercent;
        }

        private static void BossGroup_DropRewards(On.RoR2.BossGroup.orig_DropRewards orig, BossGroup group)
        {
            group.scaleRewardsByPlayerCount = false;
            group.bonusRewardCount += _bossItems - 1; // Rewards are 1 + bonusRewardCount, so we subtract one
            orig(group);
        }

        private static void SetSacrificeOffset(
            On.RoR2.Artifacts.SacrificeArtifactManager.orig_OnPrePopulateSceneServer orig, SceneDirector sceneDirector)
        {
            _sacrificeOffset = 2;
            orig(sceneDirector);
        }

        //private static void BossGroup_DropRewards(On.RoR2.BossGroup.orig_DropRewards orig, BossGroup self)
        //{
        //    ItemDropAPI.BossDropParticipatingPlayerCount = _bossItems;
        //    orig(self);
        //}

        /// <summary>
        /// // Helper function for Bossloot
        /// </summary>
        private static void OverrideBossLootScaling(On.RoR2.TeleporterInteraction.orig_OnInteractionBegin orig,
            TeleporterInteraction self, Interactor activator)
        {
            _bossItems = ShareSuite.OverrideBossLootScalingEnabled.Value
                ? ShareSuite.BossLootCredit.Value
                : Run.instance.participatingPlayerCount;
            orig(self, activator);
        }

        public static bool IsMultiplayer()
        {
            // Check whether the quantity of players in the lobby exceeds one.
            return ShareSuite.OverrideMultiplayerCheck.Value || PlayerCharacterMasterController.instances.Count > 1;
        }

        private static void InteractibleCreditOverride(On.RoR2.SceneDirector.orig_PlaceTeleporter orig,
            SceneDirector self)
        {
            Debug.Log($"Credit value before modification: {self.interactableCredit}");
            orig(self);
            int originalCredit = self.interactableCredit;

            // Below variable is called component to more resemble the original RoR2 Code.
            ClassicStageInfo component = SceneInfo.instance.GetComponent<ClassicStageInfo>();

            Debug.Log(SceneInfo.instance.sceneDef.nameToken);

            #region InteractablesCredit

            // This is the standard amount of interactablesCredit we work with.
            // Prior to the interactablesCredit overhaul this was the standard value for all runs.
            var interactableCredit = self.interactableCredit;
            var interactableCredit2 = self.interactableCredit;

            if (component)
            {
                Debug.Log("Current gamemode; " + GameModeCatalog.GetGameModeName(RoR2.Run.instance.gameModeIndex));
                if (GameModeCatalog.GetGameModeName(Run.instance.gameModeIndex) == "InfiniteTowerRun")
                {
                    // For simulacrum, make sure the interactable credit is set correctly as it is changed
                    // within the director differently than other stages, see issue #216
                    interactableCredit = 600;
                }
                else
                {
                    // Overwrite our base value with the actual amount of director credits.
                    interactableCredit = component.sceneDirectorInteractibleCredits;
                }
                
                // We require playercount for several of the following computations. We don't want this to break with
                // those crazy 'mega party mods', thus we clamp this value.
                int clampPlayerCount = Math.Min(Run.instance.participatingPlayerCount, 8);

                // Slightly increasing pickup for higher player counts.
                float num = 0.5f + (float) clampPlayerCount * 0.5f;

                // The flat creditModifier (called num by the RoR2 codebase) slightly adjust interactables based on the amount of players.
                // We do not want to reduce the amount of interactables too much for very high amounts of players (to support multiplayer mods).
                float creditModifier = (float) (0.95 + clampPlayerCount * 0.05);

                // In addition to our flat modifier, we additionally introduce a stage modifier.
                // This reduces player strength early game (as having more bodies gives a flat power increase early game).
                creditModifier *= (float) Math.Max(
                    1.0 + 0.1 * Math.Min(
                        clampPlayerCount * 2 - Run.instance.stageClearCount - 2
                        , 3)
                    , 1.0);

                // We must apply the transformation to interactableCredit otherwise bonusIntractableCreditObject will be overwritten.
                interactableCredit = (int) (interactableCredit / creditModifier);
                interactableCredit2 = (int) (interactableCredit * num / creditModifier);

                // Fetch the amount of bonus interactables we may play with. We have to do this after our first math block,
                // as we do not want to divide bonuscredits twice.
                if (component.bonusInteractibleCreditObjects != null)
                {
                    foreach (var bonusInteractableCreditObject in component.bonusInteractibleCreditObjects)
                    {
                        if (bonusInteractableCreditObject.objectThatGrantsPointsIfEnabled && bonusInteractableCreditObject.objectThatGrantsPointsIfEnabled.activeSelf)
                        {
                            interactableCredit += bonusInteractableCreditObject.points / clampPlayerCount;
                        }
                    }
                }
            }

            // Set interactables budget to interactableCredit * config player count / sacrificeOffset.
            if (ShareSuite.OverridePlayerScalingEnabled.Value &&
                (!SceneInfo.instance || !NoInteractibleOverrideScenes.Contains(SceneInfo.instance.sceneDef.nameToken)))
            {
                self.interactableCredit =
                    (int) (interactableCredit * ShareSuite.InteractablesCredit.Value / _sacrificeOffset) +
                    ShareSuite.InteractablesOffset.Value;
                interactableCredit2 = (int) (interactableCredit2 * ShareSuite.InteractablesCredit.Value / _sacrificeOffset) +
                    ShareSuite.InteractablesOffset.Value;
            }
            else
            {
                self.interactableCredit = interactableCredit;
            }

                #endregion

            _sacrificeOffset = 1;
#if DEBUG
            string line =
                $"{originalCredit};{self.interactableCredit};" +
                $"{interactableCredit2};{Run.instance.selectedDifficulty};" + 
                $"{Run.instance.participatingPlayerCount};{Run.instance.stageClearCount}";
            Debug.Log(line);
            string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "../LocalLow/Hopoo Games, LLC/Risk of Rain 2", "DataGathering.csv");
            if (System.IO.File.Exists(filePath)) {
                using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(filePath, true))
                {
                    outputFile.WriteLine(line);
                }
            } else
            {
                using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(filePath))
                {
                    outputFile.WriteLine("GameProposed Credit;interactableCredit;interactableCredit2;GameDifficult;PlayerCount;Stage");
                    outputFile.WriteLine(line);
                }
            }
#endif
        }

        private static float GetExpAdjustedDropChancePercent(On.RoR2.Util.orig_GetExpAdjustedDropChancePercent orig,
            float baseChancePercent, GameObject characterBodyObject)
        {
            if (ShareSuite.SacrificeFixEnabled.Value)
            {
                baseChancePercent /= PlayerCharacterMasterController.instances.Count;
            }

            return orig(baseChancePercent, characterBodyObject);
        }
    }
}