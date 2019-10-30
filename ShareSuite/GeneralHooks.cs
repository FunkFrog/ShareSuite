using R2API;
using RoR2;

namespace ShareSuite
{
    public static class GeneralHooks
    {
        public static int BossItems = 1;

        public static void OnPlaceTeleporter()
        {
            On.RoR2.SceneDirector.PlaceTeleporter += (orig, self) => //Replace 1 player values
            {
                orig(self);
                if (!ShareSuite.ModIsEnabled.Value) return;

                // Allow for money sharing triggers as teleporter is inactive
                MoneySharingHooks.SetTeleporterActive(false);

                // This should run on every map, as it is required to fix shared money.
                // Reset shared money value to the default (15) at the start of each round
                MoneySharingHooks.SharedMoneyValue = 15;

                #region InteractablesCredit
                
                // This is the standard amount of interactablesCredit we work with.
                // Prior to the interactablesCredit overhaul this was the standard value for all runs.
                var interactableCredit = 200;

                var component = SceneInfo.instance.GetComponent<ClassicStageInfo>();

                if (component)
                {
                    // Overwrite our base value with the actual amount of director credits.
                    interactableCredit = component.sceneDirectorInteractibleCredits;

                    // We require playercount for several of the following computations. We don't want this to break with
                    // those crazy 'mega party mods', thus we clamp this value.
                    var clampPlayerCount = System.Math.Min(Run.instance.participatingPlayerCount, 8);

                    // The flat creditModifier slightly adjust interactables based on the amount of players.
                    // We do not want to reduce the amount of interactables too much for very high amounts of players (to support multiplayer mods).
                    var creditModifier = (float) (0.95 + clampPlayerCount * 0.05);

                    // In addition to our flat modifier, we additionally introduce a stage modifier.
                    // This reduces player strength early game (as having more bodies gives a flat power increase early game).
                    creditModifier = creditModifier * (float) System.Math.Max(
                                         1.0 + 0.1 * System.Math.Min(
                                             Run.instance.participatingPlayerCount * 2 - Run.instance.stageClearCount -
                                             2, 3), 1.0);

                    // We must apply the transformation to interactableCredit otherwise bonusIntractableCreditObject will be overwritten.
                    interactableCredit = (int) (interactableCredit / creditModifier);
                    
                    // Fetch the amount of bonus interactables we may play with. We have to do this after our first math block,
                    // as we do not want to divide bonuscredits twice.
                    if (component.bonusInteractibleCreditObjects != null)
                    {
                        foreach (var bonusInteractableCreditObject in component.bonusInteractibleCreditObjects)
                        {
                            if (bonusInteractableCreditObject.objectThatGrantsPointsIfEnabled.activeSelf)
                            {
                                interactableCredit += bonusInteractableCreditObject.points / clampPlayerCount;
                            }
                        }
                    }
                }

                // Set interactables budget to interactableCredit * config player count.
                if (ShareSuite.OverridePlayerScalingEnabled.Value)
                    self.SetFieldValue("interactableCredit", interactableCredit * ShareSuite.InteractablesCredit.Value);

                #endregion
            };
        }

        public static void OnTpInteraction()
        {
            On.RoR2.TeleporterInteraction.OnInteractionBegin += (orig, self, activator) =>
            {
                #region Itemsharing
                // Helper function for Bossloot.
                if (ShareSuite.ModIsEnabled.Value && ShareSuite.OverrideBossLootScalingEnabled.Value)
                    BossItems = ShareSuite.BossLootCredit.Value;
                else
                    BossItems = Run.instance.participatingPlayerCount;

                #endregion Itemsharing

                orig(self, activator);
            };
        }

        public static void OverrideBossScaling()
        {
            On.RoR2.BossGroup.DropRewards += (orig, self) =>
            {
                if (!ShareSuite.ModIsEnabled.Value)
                {
                    orig(self);
                    return;
                }

                ItemDropAPI.BossDropParticipatingPlayerCount = BossItems;
                orig(self);
            };
        }

        public static bool IsMultiplayer()
        {
            // Check whether the quantity of players in the lobby exceeds one.
            return PlayerCharacterMasterController.instances.Count > 1;
        }
    }
}
