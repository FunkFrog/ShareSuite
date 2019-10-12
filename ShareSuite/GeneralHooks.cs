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

                var interactableCredit = 200;

                var component = SceneInfo.instance.GetComponent<ClassicStageInfo>();

                if (component)
                {
                    // Fetch the amount of interactables we may play with.
                    interactableCredit = component.sceneDirectorInteractibleCredits;
                    if (component.bonusInteractibleCreditObjects != null)
                    {
                        foreach (var bonusIntractableCreditObject in component.bonusInteractibleCreditObjects)
                        {
                            if (bonusIntractableCreditObject.objectThatGrantsPointsIfEnabled.activeSelf)
                            {
                                interactableCredit += bonusIntractableCreditObject.points;
                            }
                        }
                    }

                    // The flat creditModifier slightly adjust interactables based on the amount of players.
                    // We do not want to reduce the amount of interactables too much for very high amounts of players (to support multiplayer mods).
                    var creditModifier =
                        (float) (0.95 + System.Math.Min(Run.instance.participatingPlayerCount, 8) * 0.05);

                    // In addition to our flat modifier, we additionally introduce a stage modifier.
                    // This reduces player strength early game (as having more bodies gives a flat power increase early game).
                    creditModifier = creditModifier * (float) System.Math.Max(
                                         1.0 + 0.1 * System.Math.Min(
                                             Run.instance.participatingPlayerCount * 2 - Run.instance.stageClearCount -
                                             2, 3), 1.0);

                    // Apply the transformation. It is of paramount importance that creditModifier == 1.0 for a 1p game.
                    interactableCredit = (int) (component.sceneDirectorInteractibleCredits / creditModifier);
                }

                // Set interactables budget to 200 * config player count (normal calculation)
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
                // TODO: Shouldn't there be a ss check here?
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
            // Check whether there are more then 1 players in the lobby
            return PlayerCharacterMasterController.instances.Count > 1;
        }
    }
}