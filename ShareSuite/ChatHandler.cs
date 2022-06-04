using System;
using System.Collections.Generic;
using System.Reflection;
using RoR2;
using UnityEngine;

namespace ShareSuite
{
    public static class ChatHandler
    {
        public static void UnHook()
        {
            On.RoR2.GenericPickupController.SendPickupMessage -= RemoveDefaultPickupMessage;
            On.RoR2.Chat.SendPlayerConnectedMessage -= SendIntroMessage;
        }

        public static void Hook()
        {
            On.RoR2.GenericPickupController.SendPickupMessage += RemoveDefaultPickupMessage;
            On.RoR2.Chat.SendPlayerConnectedMessage += SendIntroMessage;
        }

        // ReSharper disable twice ArrangeTypeMemberModifiers
        private const string GrayColor = "7e91af";
        private const string RedColor = "ed4c40";
        private const string LinkColor = "5cb1ed";
        private const string ErrorColor = "ff0000";

        private const string NotSharingColor = "f07d6e";

        // Red (previously bc2525) / Blue / Yellow / Green / Orange / Cyan / Pink / Deep Purple
        private static readonly string[] PlayerColors =
            {"f23030", "2083fc", "f1f41a", "4dc344", "f27b0c", "3cdede", "db46bd", "9400ea"};

        public static void SendIntroMessage(On.RoR2.Chat.orig_SendPlayerConnectedMessage orig, NetworkUser user)
        {
            orig(user);

            if (ShareSuite.LastMessageSent.Value.Equals(ShareSuite.MessageSendVer)) return;

            ShareSuite.LastMessageSent.Value = ShareSuite.MessageSendVer;

            var notRepeatedMessage = $"<color=#{GrayColor}>(This message will </color><color=#{RedColor}>NOT</color>"
                                     + $"<color=#{GrayColor}> display again!) </color>";
            var message = $"<color=#{GrayColor}>Hey there! Thanks for installing </color>"
                          + $"<color=#{RedColor}>ShareSuite 2.7</color><color=#{GrayColor}>! We're currently"
                          + " trying to get a better idea of how people use our mod. If you wouldn't mind taking 2 minutes to"
                          + $" fill out this form, it would be </color><color=#{RedColor}>invaluable</color>"
                          + $"<color=#{GrayColor}> in helping us improve the mod!</color>";
            var linkMessage =
                $"<color=#{LinkColor}>https://tinyurl.com/sharesuite</color>    <color=#{GrayColor}>(Type into browser)</color>";
            var clickChatBox = $"<color=#{RedColor}>(Click the chat box to view the full message)</color>";

            var timer = new System.Timers.Timer(5000); // Send messages after 5 seconds
            timer.Elapsed += delegate
            {
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = notRepeatedMessage });
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = message });
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = linkMessage });
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = clickChatBox });
            };
            timer.AutoReset = false;
            timer.Start();
        }

        public static void RemoveDefaultPickupMessage(On.RoR2.GenericPickupController.orig_SendPickupMessage orig,
            CharacterMaster master, PickupIndex pickupIndex)
        {
            if (!ShareSuite.RichMessagesEnabled.Value) orig(master, pickupIndex);
        }

        public static void SendRichPickupMessage(CharacterMaster player, PickupDef pickupDef)
        {
            var body = player.hasBody ? player.GetBody() : null;

            if (!GeneralHooks.IsMultiplayer() || body == null || !ShareSuite.RichMessagesEnabled.Value)
            {
                if (ShareSuite.RichMessagesEnabled.Value) SendPickupMessage(player, pickupDef.pickupIndex);

                return;
            }

            var pickupColor = pickupDef.baseColor;
            var pickupName = Language.GetString(pickupDef.nameToken);
            var playerColor = GetPlayerColor(player.playerCharacterMasterController);
            var itemCount = player.inventory.GetItemCount(pickupDef.itemIndex);

            if (pickupDef.coinValue > 0)
            {
                var coinMessage =
                    $"<color=#{playerColor}>{body.GetUserName()}</color> <color=#{GrayColor}>picked up</color> " +
                    $"<color=#{ColorUtility.ToHtmlStringRGB(pickupColor)}>" +
                    $"{(string.IsNullOrEmpty(pickupName) ? "???" : pickupName)} ({pickupDef.coinValue})</color> <color=#{GrayColor}>for themselves.</color>";

                Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = coinMessage });

                return;
            }

            if (Blacklist.HasItem(pickupDef.itemIndex) || !ItemSharingHooks.IsValidItemPickup(pickupDef.pickupIndex))
            {
                var singlePickupMessage =
                    $"<color=#{playerColor}>{body.GetUserName()}</color> <color=#{GrayColor}>picked up</color> " +
                    $"<color=#{ColorUtility.ToHtmlStringRGB(pickupColor)}>" +
                    $"{(string.IsNullOrEmpty(pickupName) ? "???" : pickupName)} ({itemCount})</color> <color=#{GrayColor}>for themselves. </color>" +
                    $"<color=#{NotSharingColor}>(Item Set to NOT be Shared)</color>";
                Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = singlePickupMessage });
                return;
            }

            var pickupMessage =
                $"<color=#{playerColor}>{body.GetUserName()}</color> <color=#{GrayColor}>picked up</color> " +
                $"<color=#{ColorUtility.ToHtmlStringRGB(pickupColor)}>" +
                $"{(string.IsNullOrEmpty(pickupName) ? "???" : pickupName)} ({itemCount})</color> <color=#{GrayColor}>for themselves</color>" +
                $"{ItemPickupFormatter(body)}<color=#{GrayColor}>.</color>";
            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = pickupMessage });
        }

        public static void SendRichCauldronMessage(CharacterMaster player, PickupIndex index)
        {
            var body = player.hasBody ? player.GetBody() : null;

            if (!GeneralHooks.IsMultiplayer() ||
                body == null ||
                !ShareSuite.RichMessagesEnabled.Value)
            {
                if (ShareSuite.RichMessagesEnabled.Value) SendPickupMessage(player, index);

                return;
            }

            var pickupDef = PickupCatalog.GetPickupDef(index);
            var pickupColor = pickupDef.baseColor;
            var pickupName = Language.GetString(pickupDef.nameToken);
            var playerColor = GetPlayerColor(player.playerCharacterMasterController);
            var itemCount = player.inventory.GetItemCount(pickupDef.itemIndex);

            var pickupMessage =
                $"<color=#{playerColor}>{body.GetUserName()}</color> <color=#{GrayColor}>traded for</color> " +
                $"<color=#{ColorUtility.ToHtmlStringRGB(pickupColor)}>" +
                $"{(string.IsNullOrEmpty(pickupName) ? "???" : pickupName)} ({itemCount})</color><color=#{GrayColor}>.</color>";
            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = pickupMessage });
        }

        public static void SendRichRandomizedPickupMessage(CharacterMaster origPlayer, PickupDef origPickup,
            Dictionary<CharacterMaster, PickupDef> pickupIndices)
        {
            if (!GeneralHooks.IsMultiplayer() || !ShareSuite.RichMessagesEnabled.Value)
            {
                if (ShareSuite.RichMessagesEnabled.Value) SendPickupMessage(origPlayer, origPickup.pickupIndex);

                return;
            }

            // If nobody got a randomized item
            if (pickupIndices.Count == 1)
            {
                SendRichPickupMessage(origPlayer, origPickup);
                return;
            }

            var remainingPlayers = pickupIndices.Count;
            var pickupMessage = "";

            foreach (var index in pickupIndices)
            {
                var pickupColor = index.Value.baseColor;
                var pickupName = Language.GetString(index.Value.nameToken);
                var playerColor = GetPlayerColor(index.Key.playerCharacterMasterController);
                var itemCount =
                    index.Key.playerCharacterMasterController.master.inventory.GetItemCount(index.Value.itemIndex);

                if (remainingPlayers != pickupIndices.Count)
                {
                    if (remainingPlayers > 1)
                    {
                        pickupMessage += $"<color=#{GrayColor}>,</color> ";
                    }
                    else if (remainingPlayers == 1)
                    {
                        pickupMessage += $"<color=#{GrayColor}>, and</color> ";
                    }
                }

                remainingPlayers--;

                pickupMessage +=
                    $"<color=#{playerColor}>{index.Key.playerCharacterMasterController.GetDisplayName()}</color> " +
                    $"<color=#{GrayColor}>got</color> " +
                    $"<color=#{ColorUtility.ToHtmlStringRGB(pickupColor)}>" +
                    $"{(string.IsNullOrEmpty(pickupName) ? "???" : pickupName)} ({itemCount})</color>";
            }

            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = pickupMessage });
        }

        private static string ItemPickupFormatter(CharacterBody body)
        {
            // Initialize an int for the amount of players eligible to receive the item
            var eligiblePlayers = GetEligiblePlayers(body);

            // If there's nobody else, return " and No-one Else"
            if (eligiblePlayers < 1) return $" <color=#{GrayColor}>and no-one else</color>";

            // If there's only one other person in the lobby
            if (eligiblePlayers == 1)
            {
                // Loop through every player in the lobby
                foreach (var playerCharacterMasterController in PlayerCharacterMasterController.instances)
                {
                    var master = playerCharacterMasterController.master;

                    // If they don't have a body or are the one who picked up the item, go to the next person
                    if (!master.hasBody || master.GetBody() == body) continue;

                    // Get the player color
                    var playerColor = GetPlayerColor(playerCharacterMasterController);

                    // If the player is alive OR dead and deadplayersgetitems is on, return their name
                    return $" <color=#{GrayColor}>and</color> " + $"<color=#{playerColor}>" +
                           playerCharacterMasterController.GetDisplayName() + "</color>";
                }

                // Shouldn't happen ever, if something's borked
                return $"<color=#{ErrorColor}>???</color>";
            }

            // Initialize the return string
            var returnStr = "";

            // Loop through every player in the lobby
            foreach (var playerCharacterMasterController in PlayerCharacterMasterController.instances)
            {
                var master = playerCharacterMasterController.master;
                // If they don't have a body or are the one who picked up the item, go to the next person
                if (!master.hasBody || master.GetBody() == body) continue;

                // If the player is dead/deadplayersgetitems is off, continue and add nothing
                if (master.IsDeadAndOutOfLivesServer() && !ShareSuite.DeadPlayersGetItems.Value) continue;

                // Get the player color
                var playerColor = GetPlayerColor(playerCharacterMasterController);

                // If the amount of players remaining is more then one (not the last)
                if (eligiblePlayers > 1)
                {
                    returnStr += $"<color=#{GrayColor}>,</color> ";
                }
                else if (eligiblePlayers == 1) // If it is the last player remaining
                {
                    returnStr += $"<color=#{GrayColor}>, and</color> ";
                }

                eligiblePlayers--;

                // If the player is alive OR dead and deadplayersgetitems is on, add their name to returnStr
                returnStr += $"<color=#{playerColor}>" + playerCharacterMasterController.GetDisplayName() + "</color>";
            }

            // Return the string
            return returnStr;
        }

        // Returns the player color as a hex string w/o the #
        private static string GetPlayerColor(PlayerCharacterMasterController controllerMaster)
        {
            var playerLocation = PlayerCharacterMasterController.instances.IndexOf(controllerMaster);
            return PlayerColors[playerLocation % 8];
        }

        private static int GetEligiblePlayers(CharacterBody body)
        {
            var eligiblePlayers = 0;

            foreach (var playerCharacterMasterController in PlayerCharacterMasterController.instances)
            {
                var master = playerCharacterMasterController.master;
                // If they don't have a body or are the one who picked up the item, go to the next person
                if (!master.inventory || master.GetBody() == body) continue;

                // If the player is alive, add one to eligablePlayers
                if (!master.IsDeadAndOutOfLivesServer() || ShareSuite.DeadPlayersGetItems.Value)
                {
                    eligiblePlayers++;
                }
            }

            return eligiblePlayers;
        }

        public delegate void SendPickupMessageDelegate(CharacterMaster master, PickupIndex pickupIndex);

        public static readonly SendPickupMessageDelegate SendPickupMessage =
            (SendPickupMessageDelegate) Delegate.CreateDelegate(typeof(SendPickupMessageDelegate),
                typeof(GenericPickupController).GetMethod("SendPickupMessage",
                    BindingFlags.Public | BindingFlags.Static) ?? throw new MissingMethodException("Unable to find SendPickupMessage"));
    }
}