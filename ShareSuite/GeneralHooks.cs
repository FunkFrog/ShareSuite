using RoR2;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ShareSuite
{
    public static class GeneralHooks
    {
        private static int _sacrificeOffset = 1;

        private static List<string> NoInteractibleOverrideScenes = new List<string>
        {
            "MAP_BAZAAR_TITLE",
            "MAP_ARENA_TITLE", "MAP_LIMBO_TITLE", "MAP_MYSTERYSPACE_TITLE"
        };

        internal static void Hook()
        {
            On.RoR2.BossGroup.DropRewards += BossGroup_DropRewards;
            On.RoR2.SceneDirector.PlaceTeleporter += InteractibleCreditOverride;
            On.RoR2.Artifacts.SacrificeArtifactManager.OnPrePopulateSceneServer += SetSacrificeOffset;
            On.RoR2.Util.GetExpAdjustedDropChancePercent += GetExpAdjustedDropChancePercent;
        }

        internal static void UnHook()
        {
            On.RoR2.BossGroup.DropRewards -= BossGroup_DropRewards;
            On.RoR2.SceneDirector.PlaceTeleporter -= InteractibleCreditOverride;
            On.RoR2.Artifacts.SacrificeArtifactManager.OnPrePopulateSceneServer -= SetSacrificeOffset;
            On.RoR2.Util.GetExpAdjustedDropChancePercent -= GetExpAdjustedDropChancePercent;
        }

        private static void BossGroup_DropRewards(On.RoR2.BossGroup.orig_DropRewards orig, BossGroup group)
        {
            // Swap the player count the rewards are multiplied by for our credit, so mountain shrines still multiply
            if (ShareSuite.OverrideBossLootScalingEnabled.Value && group.scaleRewardsByPlayerCount)
            {
                group.scaleRewardsByPlayerCount = false;
                // Rewards are 1 + bonusRewardCount, so we subtract one
                group.bonusRewardCount =
                    (1 + group.bonusRewardCount) * Math.Max(ShareSuite.BossLootCredit.Value, 0) - 1;
            }

            orig(group);
        }

        private static void SetSacrificeOffset(
            On.RoR2.Artifacts.SacrificeArtifactManager.orig_OnPrePopulateSceneServer orig, SceneDirector sceneDirector)
        {
            _sacrificeOffset = 2;
            orig(sceneDirector);
        }

        public static bool IsMultiplayer()
        {
            // Check whether the quantity of players in the lobby exceeds one.
            return ShareSuite.OverrideMultiplayerCheck.Value || PlayerCharacterMasterController.instances.Count > 1;
        }

        private static void InteractibleCreditOverride(On.RoR2.SceneDirector.orig_PlaceTeleporter orig,
            SceneDirector self)
        {
            orig(self);

            Debug.Log(SceneInfo.instance.sceneDef.nameToken);

            #region InteractablesCredit

            // This is the standard amount of interactablesCredit we work with.
            // Prior to the interactablesCredit overhaul this was the standard value for all runs.
            var interactableCredit = 200;
            var origSelfInteractableCredit = self.interactableCredit;

            var stageInfo = SceneInfo.instance.GetComponent<ClassicStageInfo>();

            if (stageInfo)
            {
                Log.Debug("Current gamemode; " + GameModeCatalog.GetGameModeName(RoR2.Run.instance.gameModeIndex));
                if (GameModeCatalog.GetGameModeName(Run.instance.gameModeIndex) == "InfiniteTowerRun")
                {
                    // For simulacrum, make sure the interactable credit is set correctly as it is changed
                    // within the director differently than other stages, see issue #216
                    interactableCredit = 600;
                }
                else
                {
                    // Overwrite our base value with the actual amount of director credits.
                    interactableCredit = stageInfo.sceneDirectorInteractibleCredits;
                }

                // We require playercount for several of the following computations. We don't want this to break with
                // those crazy 'mega party mods', thus we clamp this value.
                var clampPlayerCount = Math.Min(Run.instance.participatingPlayerCount, 8);

                // The flat creditModifier slightly adjust interactables based on the amount of players.
                // We do not want to reduce the amount of interactables too much for very high amounts of players (to support multiplayer mods).
                var creditModifier = (float) (0.95 + clampPlayerCount * 0.05);

                // In addition to our flat modifier, we additionally introduce a stage modifier.
                // This reduces player strength early game (as having more bodies gives a flat power increase early game).
                creditModifier *= (float) Math.Max(
                    1.0 + 0.1 * Math.Min(
                        Run.instance.participatingPlayerCount * 2 - Run.instance.stageClearCount - 2
                        , 3)
                    , 1.0);

                // We must apply the transformation to interactableCredit otherwise bonusIntractableCreditObject will be overwritten.
                interactableCredit = (int) (interactableCredit / creditModifier);

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

            // Set interactables budget to interactableCredit * config player count / sacrificeOffset.
            if (ShareSuite.OverridePlayerScalingEnabled.Value &&
                (!SceneInfo.instance || !NoInteractibleOverrideScenes.Contains(SceneInfo.instance.sceneDef.nameToken)))
            {
                self.interactableCredit =
                    (int) (interactableCredit * ShareSuite.InteractablesCredit.Value / _sacrificeOffset) +
                    ShareSuite.InteractablesOffset.Value;
            }

            #endregion

            _sacrificeOffset = 1;
            string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "../LocalLow/Hopoo Games, LLC/Risk of Rain 2", "DataGathering.csv");

            string message =
                $"{origSelfInteractableCredit};" + // GameProposed Credit
                $"{self.interactableCredit};" + // interactableCredit
                $"{interactableCredit};" +  // interactableCredit2
                $"{Run.instance.selectedDifficulty};" + // GameDifficulty
                $"{Run.instance.participatingPlayerCount};" + // PlayerCount
                $"{Run.instance.stageClearCount};" + // Stage
                $"{SceneCatalog.mostRecentSceneDef.baseSceneName};" + // SceneName
                $"{ShareSuite.OverridePlayerScalingEnabled.Value};" + // OverridePlayerScalingEnabled
                $"{ShareSuite.InteractablesCredit.Value};" + // InteractablesCredit
                $"{ShareSuite.InteractablesOffset.Value}"; // InteractablesOffset
            Debug.Log($"ShareSuite: DataGathering: {message}");
            try
            {
                if (File.Exists(path))
                {
                    using (StreamWriter streamWriter = new StreamWriter(path, true))
                        streamWriter.WriteLine(message);
                }
                else
                {
                    using (StreamWriter streamWriter = new StreamWriter(path))
                    {
                        streamWriter.WriteLine("GameProposed Credit;interactableCredit;interactableCredit2;" +
                            "GameDifficulty;PlayerCount;Stage;SceneName;" +
                            "OverridePlayerScalingEnabled;InteractablesCredit;InteractablesOffset");
                        streamWriter.WriteLine(message);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to write to DataGathering.csv: {e.Message}");
            }
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