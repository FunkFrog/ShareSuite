using BepInEx;
using RoR2;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ShareSuite
{
    public class CommandHelper
    {
        public static void RegisterCommands(RoR2.Console self)
        {
            var types = typeof(CommandHelper).Assembly.GetTypes();
            var catalog = self.GetFieldValue<IDictionary>("concommandCatalog");

            foreach (var methodInfo in types.SelectMany(x => x.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)))
            {
                var customAttributes = methodInfo.GetCustomAttributes(false);
                foreach (var attribute in customAttributes.OfType<ConCommandAttribute>())
                {
                    var conCommand = Reflection.GetNestedType<RoR2.Console>("ConCommand").Instantiate();

                    conCommand.SetFieldValue("flags", attribute.flags);
                    conCommand.SetFieldValue("helpText", attribute.helpText);
                    conCommand.SetFieldValue("action", (RoR2.Console.ConCommandDelegate)Delegate.CreateDelegate(typeof(RoR2.Console.ConCommandDelegate), methodInfo));

                    catalog[attribute.commandName.ToLower()] = conCommand;
                }
            }
        }
    }

    [BepInPlugin("com.funkfrog_sipondo.sharesuite", "ShareSuite", "1.2.0")]
    public class ShareSuite : BaseUnityPlugin
    {
        public static bool WrapMoneyIsShared;
        public static int WrapMoneyScalar;
        public static bool WrapWhiteItemsShared;
        public static bool WrapGreenItemsShared;
        public static bool WrapRedItemsShared;
        public static bool WrapLunarItemsShared;
        public static bool WrapBossItemsShared;
        public static bool WrapQueensGlandsShared;
        public static bool WrapPrinterCauldronFixEnabled;
        public static bool WrapDisablePlayerScalingEnabled;
        public static int  WrapInteractablesCredit;
        public static bool WrapDisableBossLootScalingEnabled;
        public static int WrapBossLootCredit;

        public ShareSuite()
        {
            InitWrap();
            On.RoR2.Console.Awake += (orig, self) =>
            {
                CommandHelper.RegisterCommands(self);
                orig(self);
            };
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
            
            WrapMoneyScalar = Config.Wrap(
                "Settings",
                "MoneyScalar",
                "Modifies percent of gold earned when money sharing is on.",
                100).Value / 100;
            
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

        // MoneyIsShared
        [ConCommand(commandName = "ss_MoneyIsShared", flags = ConVarFlags.None, helpText = "Modifies whether money is shared or not.")]
        private static void CCMoneyIsShared(ConCommandArgs args)
        {
            if (args.Count != 1 || !bool.TryParse(args[0], out WrapMoneyIsShared))
                Debug.Log("Invalid arguments.");
            else
                Debug.Log($"Money sharing set to {WrapMoneyIsShared}.");
        }

        // MoneyScalar
        [ConCommand(commandName = "ss_MoneyScalar", flags = ConVarFlags.None, helpText = "Modifies percent of gold earned when money sharing is on.")]
        private static void CCMoneyScalar(ConCommandArgs args)
        {
            if (args.Count != 1 || !int.TryParse(args[0], out WrapMoneyScalar))
                Debug.Log("Invalid arguments.");
            else
                Debug.Log($"Money multiplier set to {WrapMoneyScalar}.");
        }

        // WhiteItemsShared
        [ConCommand(commandName = "ss_WhiteItemsShared", flags = ConVarFlags.None, helpText = "Modifies whether white items are shared or not.")]
        private static void CCWhiteShared(ConCommandArgs args)
        {
            if (args.Count != 1 || !bool.TryParse(args[0], out WrapWhiteItemsShared))
                Debug.Log("Invalid arguments.");
            else
                Debug.Log($"White item sharing set to {WrapWhiteItemsShared}.");
        }

        // GreenItemsShared
        [ConCommand(commandName = "ss_GreenItemsShared", flags = ConVarFlags.None, helpText = "Modifies whether green items are shared or not.")]
        private static void CCGreenShared(ConCommandArgs args)
        {
            if (args.Count != 1 || !bool.TryParse(args[0], out WrapGreenItemsShared))
                Debug.Log("Invalid arguments.");
            else
                Debug.Log($"Green item sharing set to {WrapGreenItemsShared}.");
        }

        // RedItemsShared
        [ConCommand(commandName = "ss_RedItemsShared", flags = ConVarFlags.None, helpText = "Modifies whether red items are shared or not.")]
        private static void CCRedShared(ConCommandArgs args)
        {
            if (args.Count != 1 || !bool.TryParse(args[0], out WrapRedItemsShared))
                Debug.Log("Invalid arguments.");
            else
                Debug.Log($"Red item sharing set to {WrapRedItemsShared}.");
        }

        // LunarItemsShared
        [ConCommand(commandName = "ss_LunarItemsShared", flags = ConVarFlags.None, helpText = "Modifies whether lunar items are shared or not.")]
        private static void CCLunarShared(ConCommandArgs args)
        {
            if (args.Count != 1 || !bool.TryParse(args[0], out WrapLunarItemsShared))
                Debug.Log("Invalid arguments.");
            else
                Debug.Log($"Lunar item sharing set to {WrapLunarItemsShared}.");
        }

        // BossItemsShared
        [ConCommand(commandName = "ss_BossItemsShared", flags = ConVarFlags.None, helpText = "Modifies whether boss items are shared or not.")]
        private static void CCBossShared(ConCommandArgs args)
        {
            if (args.Count != 1 || !bool.TryParse(args[0], out WrapBossItemsShared))
                Debug.Log("Invalid arguments.");
            else
                Debug.Log($"Boss item sharing set to {WrapBossItemsShared}.");
        }

        // QueensGlandsShared
        [ConCommand(commandName = "ss_QueensGlandsShared", flags = ConVarFlags.None, helpText = "Modifies whether Queens Glands are shared or not.")]
        private static void CCQueenShared(ConCommandArgs args)
        {
            if (args.Count != 1 || !bool.TryParse(args[0], out WrapQueensGlandsShared))
                Debug.Log("Invalid arguments.");
            else
                Debug.Log($"Queens Gland sharing set to {WrapQueensGlandsShared}.");
        }

        // PrinterCauldronFix
        [ConCommand(commandName = "ss_PrinterCauldronFix", flags = ConVarFlags.None, helpText = "Modifies whether printers and cauldrons should not duplicate items.")]
        private static void CCPrinterCauldronFix(ConCommandArgs args)
        {
            if (args.Count != 1 || !bool.TryParse(args[0], out WrapPrinterCauldronFixEnabled))
                Debug.Log("Invalid arguments.");
            else
                Debug.Log($"Printer and cauldron fix set to {WrapPrinterCauldronFixEnabled}.");
        }

        // DisablePlayerScaling
        [ConCommand(commandName = "ss_DisablePlayerScaling", flags = ConVarFlags.None, helpText = "Modifies whether interactable count should scale based on player count.")]
        private static void CCDisablePlayerScaling (ConCommandArgs args)
        {
            if (args.Count != 1 || !bool.TryParse(args[0], out WrapDisablePlayerScalingEnabled))
                Debug.Log("Invalid arguments.");
            else
                Debug.Log($"Player scaling disable set to {WrapDisablePlayerScalingEnabled}.");
        }

        // InteractablesCredit
        [ConCommand(commandName = "ss_InteractablesCredit", flags = ConVarFlags.None, helpText = "Modifies amount of interactables when player scaling is disabled.")]
        private static void CCInteractablesCredit(ConCommandArgs args)
        {
            if (args.Count != 1 || !int.TryParse(args[0], out WrapInteractablesCredit))
                Debug.Log("Invalid arguments.");
            else
                Debug.Log($"Interactables multiplier set to {WrapInteractablesCredit}.");
        }

        // DisableBossLootScaling
        [ConCommand(commandName = "ss_DisableBossLootScaling", flags = ConVarFlags.None, helpText = "Modifies whether boss loot should scale based on player count.")]
        private static void CCBossLoot(ConCommandArgs args)
        {
            if (args.Count != 1 || !bool.TryParse(args[0], out WrapDisableBossLootScalingEnabled))
                Debug.Log("Invalid arguments.");
            else
                Debug.Log($"Boss loot scaling disable set to {WrapDisableBossLootScalingEnabled}.");
        }

        // BossLootCredit
        [ConCommand(commandName = "ss_BossLootCredit", flags = ConVarFlags.None, helpText = "Modifies amount of boss item drops.")]
        private static void CCBossLootCredit(ConCommandArgs args)
        {
            if (args.Count != 1 || !int.TryParse(args[0], out WrapBossLootCredit))
                Debug.Log("Invalid arguments.");
            else
                Debug.Log($"Boss loot multiplier set to {WrapBossLootCredit}.");
                Hooks.
        }
    }
}