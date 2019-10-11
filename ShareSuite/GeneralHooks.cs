using R2API;
using RoR2;
using UnityEngine.SceneManagement;

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

                // This should run on every map, as it is required to fix shared money.
                // Reset shared money value to the default (15) at the start of each round
                MoneySharingHooks.SharedMoneyValue = 15;

                bool goldshores = SceneManager.GetActiveScene().name == "goldshores";
                bool mysteryspace = SceneManager.GetActiveScene().name == "mysteryspace";

                if (goldshores || mysteryspace)
                    return;

                // Set interactables budget to 200 * config player count (normal calculation)
                if (ShareSuite.OverridePlayerScalingEnabled.Value)
                    self.SetFieldValue("interactableCredit", 200 * ShareSuite.InteractablesCredit.Value);
            };
        }

        public static void OnTpInteraction()
        {
            On.RoR2.TeleporterInteraction.OnInteractionBegin += (orig, self, activator) =>
            {
                if (ShareSuite.ModIsEnabled.Value && ShareSuite.OverrideBossLootScalingEnabled.Value)
                    BossItems = ShareSuite.BossLootCredit.Value;
                else
                    BossItems = Run.instance.participatingPlayerCount;

                if (self.isCharged && ShareSuite.MoneyIsShared.Value)
                    MoneySharingHooks.AdjustTpMoney();

                orig(self, activator);
            };
        }

        public static void OverrideBossScaling()
        {
            On.RoR2.BossGroup.DropRewards += (orig, self) =>
            {
                ItemDropAPI.BossDropParticipatingPlayerCount = BossItems;
                orig(self);
            };
        }

        public static bool IsMultiplayer()
        {
            // Check if there are more then 1 players in the lobby
            return PlayerCharacterMasterController.instances.Count > 1;
        }
    }
}