using BepInEx;

namespace ShareSuite
{
    [BepInPlugin("com.funkfrog_sipondo.sharesuite", "ShareSuite", "1.2.0")]
    public class ShareSuite : BaseUnityPlugin
    {
        public static bool WrapMoneyIsShared { get; private set; }
        public static bool WrapWhiteItemsShared{ get; private set; }
        public static bool WrapGreenItemsShared { get; private set; }
        public static bool WrapRedItemsShared { get; private set; }
        public static bool WrapLunarItemsShared { get; private set; }
        public static bool WrapBossItemsShared { get; private set; }
        public static bool WrapQueensGlandsShared { get; private set; }
        public static bool WrapPrinterCauldronFixEnabled { get; private set; }
        public static bool WrapDisablePlayerScalingEnabled { get; private set; }
        public static int  WrapInteractablesCredit { get; private set; }
        public static bool WrapDisableBossLootScalingEnabled { get; private set; }
        public static int WrapBossLootCredit { get; private set; }

        public ShareSuite()
        {
            InitWrap();
            // Register all the hooks
            Hooks.OnGrantItem();
            Hooks.OnShopPurchase();
            Hooks.OnPurchaseDrop();
            Hooks.DisableInteractablesScaling();
            Hooks.ModifyGoldReward();
        }

        public void InitWrap()
        {
            // Add config options for all settings
            WrapMoneyIsShared = Config.Wrap(
                "Settings",
                "MoneyShared",
                "Toggles money sharing.",
                false).Value;
            
            WrapWhiteItemsShared = Config.Wrap(
                "Settings",
                "WhiteItemsShared",
                "Toggles item sharing for common items.",
                true).Value;

            WrapGreenItemsShared = Config.Wrap(
                "Settings",
                "GreenItemsShared",
                "Toggles item sharing for rare items.",
                true).Value;

            WrapRedItemsShared = Config.Wrap(
                "Settings",
                "RedItemsShared",
                "Toggles item sharing for legendary items.",
                true).Value;

            WrapLunarItemsShared = Config.Wrap(
                "Settings",
                "LunarItemsShared",
                "Toggles item sharing for Lunar items.",
                false).Value;

            WrapBossItemsShared = Config.Wrap(
                "Settings",
                "BossItemsShared",
                "Toggles item sharing for boss items.",
                true).Value;
            
            WrapQueensGlandsShared = Config.Wrap(
                "Balance",
                "QueensGlandsShared",
                "Toggles item sharing for specifically the Queen's Gland (reduces possible lag).",
                false).Value;

            WrapPrinterCauldronFixEnabled = Config.Wrap(
                "Balance",
                "PrinterCauldronFix",
                "Toggles 3D printer and Cauldron item dupe fix by giving the item directly instead of" +
                " dropping it on the ground.",
                true).Value;

            WrapDisablePlayerScalingEnabled = Config.Wrap(
                "Balance",
                "DisablePlayerScaling",
                "Toggles scaling of the amount of interactables (chests, shrines, etc) that spawn in the world.",
                true).Value;

            WrapInteractablesCredit = Config.Wrap(
                "Balance",
                "InteractablesCredit",
                "If player scaling via this mod is enabled, the amount of players the game should think are playing in terms of chest spawns.",
                1).Value;

            WrapDisableBossLootScalingEnabled = Config.Wrap(
                "Balance",
                "DisableBossLootScaling",
                "Toggles scaling of the amount of boss loot that drops to one player.",
                true).Value;

            WrapBossLootCredit = Config.Wrap(
                "Balance",
                "BossLootCredit",
                "If boss loot scaling via this mod is enabled, the amount of items the teleporter will drop by default.",
                1).Value;
            
        }
    }
}