using BepInEx;

namespace ShareSuite
{
    [BepInPlugin("com.funkfrog_sipondo.sharesuite", "ShareSuite", "1.1.0")]
    public class ShareSuite : BaseUnityPlugin
    {
        public static bool Wrap_MoneyIsShared { get; private set; }
        public static bool Wrap_WhiteItemsShared{ get; private set; }
        public static bool Wrap_GreenItemsShared { get; private set; }
        public static bool Wrap_RedItemsShared { get; private set; }
        public static bool Wrap_LunarItemsShared { get; private set; }
        public static bool Wrap_BossItemsShared { get; private set; }
        public static bool Wrap_QueensGlandsShared { get; private set; }
        public static bool Wrap_PrinterCauldronFixEnabled { get; private set; }
        public static bool Wrap_DisablePlayerScalingEnabled { get; private set; }
        public static int  Wrap_InteractablesCredit { get; private set; }
        public static bool Wrap_DisableBossLootScalingEnabled { get; private set; }
        public static int Wrap_BossLootCredit { get; private set; }

        public ShareSuite()
        {
            InitWrap();
            // Register all the hooks
            Hooks.OnGrantItem();
            Hooks.OnShopPurchase();
            Hooks.OnPurchaseDrop();
            Hooks.DisableInteractablesScaling();
        }

        public void InitWrap()
        {
            // Add config options for all settings
            Wrap_MoneyIsShared = Config.Wrap(
                "Settings",
                "MoneyShared",
                "Toggles money sharing.",
                false).Value;
            
            Wrap_WhiteItemsShared = Config.Wrap(
                "Settings",
                "WhiteItemsShared",
                "Toggles item sharing for common items.",
                true).Value;

            Wrap_GreenItemsShared = Config.Wrap(
                "Settings",
                "GreenItemsShared",
                "Toggles item sharing for rare items.",
                true).Value;

            Wrap_RedItemsShared = Config.Wrap(
                "Settings",
                "RedItemsShared",
                "Toggles item sharing for legendary items.",
                true).Value;

            Wrap_LunarItemsShared = Config.Wrap(
                "Settings",
                "LunarItemsShared",
                "Toggles item sharing for Lunar items.",
                false).Value;

            Wrap_BossItemsShared = Config.Wrap(
                "Settings",
                "BossItemsShared",
                "Toggles item sharing for boss items.",
                true).Value;
            
            Wrap_QueensGlandsShared = Config.Wrap(
                "Balance",
                "QueensGlandsShared",
                "Toggles item sharing for specifically the Queen's Gland (reduces possible lag).",
                false).Value;

            Wrap_PrinterCauldronFixEnabled = Config.Wrap(
                "Balance",
                "PrinterCauldronFix",
                "Toggles 3D printer and Cauldron item dupe fix by giving the item directly instead of" +
                " dropping it on the ground.",
                true).Value;

            Wrap_DisablePlayerScalingEnabled = Config.Wrap(
                "Balance",
                "DisablePlayerScaling",
                "Toggles scaling of the amount of interactables (chests, shrines, etc) that spawn in the world.",
                true).Value;

            Wrap_InteractablesCredit = Config.Wrap(
                "Balance",
                "InteractablesCredit",
                "If player scaling via this mod is enabled, the amount of players the game should think are playing in terms of chest spawns.",
                1).Value;

            Wrap_DisableBossLootScalingEnabled = Config.Wrap(
                "Balance",
                "DisableBossLootScaling",
                "Toggles scaling of the amount of boss loot that drops to one player.",
                true).Value;

            Wrap_BossLootCredit = Config.Wrap(
                "Balance",
                "BossLootCredit",
                "If boss loot scaling via this mod is enabled, the amount of items the teleporter will drop by default.",
                1).Value;
            
        }
    }
}