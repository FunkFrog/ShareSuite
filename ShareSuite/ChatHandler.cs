using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using R2API.MiscHelpers;
using RoR2;
using ShareSuite.Extensions;
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

        private static readonly string[] PlayerColors =
        {
            "f23030", // Red (previously bc2525)
            "2083fc", // Blue
            "f1f41a", // Yellow
            "4dc344", // Green
            "f27b0c", // Orange
            "3cdede", // Cyan
            "db46bd", // Pink
            "9400ea" // Deep Purple
        };

        public static void SendIntroMessage(On.RoR2.Chat.orig_SendPlayerConnectedMessage orig, NetworkUser user)
        {
            orig(user);

            if (ShareSuite.LastMessageSent.Value.Equals(ShareSuite.MessageSendVer))
            {
                return;
            }

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
            if (!ShareSuite.RichMessagesEnabled.Value)
            {
                orig(master, pickupIndex);
            }
        }

        public static void SendRichPickupMessage(CharacterMaster player, PickupDef pickupDef)
        {
            var body = player.hasBody ? player.GetBody() : null;

            if (!GeneralHooks.IsMultiplayer() ||
                body == null ||
                !ShareSuite.RichMessagesEnabled.Value)
            {
                if (ShareSuite.RichMessagesEnabled.Value)
                {
                    SendPickupMessage(player, pickupDef.pickupIndex);
                }

                return;
            }

            var characterName = CharacterNameWithColorFormatter(player.playerCharacterMasterController);

            var message = $"{characterName} <color=#{GrayColor}>picked up</color> ";

            if (pickupDef.coinValue > 0)
            {
                var pickupColor = ColorUtility.ToHtmlStringRGB(pickupDef.baseColor);
                var pickupName = Language.GetString(pickupDef.nameToken);

                message += $"<color=#{pickupColor}>{pickupName} ({pickupDef.coinValue})</color> <color=#{GrayColor}>for themselves.</color>";
            }
            else
            {
                var itemName = ItemNameWithInventoryCountFormatter(player, pickupDef);

                if (Blacklist.HasItem(pickupDef.itemIndex) ||
                    !ItemSharingHooks.IsValidItemPickup(pickupDef.pickupIndex))
                {
                    message += $"{itemName} <color=#{GrayColor}>for themselves.</color> <color=#{NotSharingColor}>(Item Set to NOT be Shared)</color>";
                }
                else
                {
                    message += $"{itemName} <color=#{GrayColor}>for themselves</color>{ItemPickupFormatter(body)}<color=#{GrayColor}>.</color>";
                }
            }

            Chat.SendBroadcastChat(new Chat.SimpleChatMessage {baseToken = message});
        }

        public static void SendShareVoidItemsAsBaseMessage(CharacterMaster player, PickupDef pickupDef, ItemDef itemDef)
        {
            var body = player.hasBody ? player.GetBody() : null;

            if (!GeneralHooks.IsMultiplayer() ||
                body == null ||
                !ShareSuite.RichMessagesEnabled.Value)
            {
                if (ShareSuite.RichMessagesEnabled.Value)
                {
                    SendPickupMessage(player, pickupDef.pickupIndex);
                }

                return;
            }

            var itemName = ItemNameWithInventoryCountFormatter(player, pickupDef);
            var characterName = CharacterNameWithColorFormatter(player.playerCharacterMasterController);
            var voidPickupColor = ColorUtility.ToHtmlStringRGB(itemDef.GetPickupDef().baseColor);
            var voidBaseItemName = Language.GetString(itemDef.nameToken);

            var message = $"{characterName} <color=#{GrayColor}>picked up</color> {itemName} <color=#{GrayColor}>for themselves</color>";

            var eligiblePlayers = GetEligiblePlayers(body);
            if (!eligiblePlayers.Any())
            {
                message += $" <color=#{GrayColor}>and no-one else</color>";
            }
            else
            {
                message += $", and <color=#{voidPickupColor}>{voidBaseItemName}</color> for ";
                for (var i = eligiblePlayers.Count - 1; i >= 0; i--)
                {
                    message += CharacterNameWithColorFormatter(eligiblePlayers[i]);

                    if (i > 0)
                    {
                        message += ", ";
                    }

                    if (i == 1)
                    {
                        message += $"<color=#{GrayColor}>and</color> ";
                    }
                }
            }

            message += $"<color=#{GrayColor}>.</color>";

            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = message });
        }

        public static void SendRichCauldronMessage(CharacterMaster player, PickupIndex index)
        {
            var body = player.hasBody ? player.GetBody() : null;

            if (!GeneralHooks.IsMultiplayer() ||
                body == null ||
                !ShareSuite.RichMessagesEnabled.Value)
            {
                if (ShareSuite.RichMessagesEnabled.Value)
                {
                    SendPickupMessage(player, index);
                }

                return;
            }

            var pickupDef = PickupCatalog.GetPickupDef(index);
            var characterName = CharacterNameWithColorFormatter(player.playerCharacterMasterController);
            var itemString = ItemNameWithInventoryCountFormatter(player.playerCharacterMasterController.master, pickupDef);

            var pickupMessage = $"{characterName} <color=#{GrayColor}>traded for</color> {itemString}<color=#{GrayColor}>.</color>";
            Chat.SendBroadcastChat(new Chat.SimpleChatMessage {baseToken = pickupMessage});
        }

        public static void SendRichRandomizedPickupMessage(CharacterMaster origPlayer, PickupDef origPickup,
            Dictionary<CharacterMaster, PickupDef> pickupIndices)
        {
            if (!GeneralHooks.IsMultiplayer() || !ShareSuite.RichMessagesEnabled.Value)
            {
                if (ShareSuite.RichMessagesEnabled.Value)
                {
                    SendPickupMessage(origPlayer, origPickup.pickupIndex);
                }

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

            foreach (var (key, value) in pickupIndices)
            {
                var characterName = CharacterNameWithColorFormatter(key.playerCharacterMasterController);
                var itemName = ItemNameWithInventoryCountFormatter(key.playerCharacterMasterController.master, value);

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

                pickupMessage += $"{characterName} <color=#{GrayColor}>got</color> {itemName}";
            }

            Chat.SendBroadcastChat(new Chat.SimpleChatMessage { baseToken = pickupMessage });
        }

        private static string ItemPickupFormatter(CharacterBody body)
        {
            // Find the eligible players to receive the item
            var eligiblePlayers = GetEligiblePlayers(body);

            if (!eligiblePlayers.Any())
            {
                return $" <color=#{GrayColor}>and no-one else</color>";
            }

            var returnStr = "";

            for (var i = eligiblePlayers.Count - 1; i >= 0; i--)
            {
                returnStr += ", ";

                if (i == 0)
                {
                    returnStr += $"<color=#{GrayColor}>and</color> ";
                }

                returnStr += CharacterNameWithColorFormatter(eligiblePlayers[i]);
            }

            return returnStr;
        }

        private static string ItemNameWithInventoryCountFormatter(CharacterMaster master, PickupDef pickupDef)
        {
            var pickupColor = ColorUtility.ToHtmlStringRGB(pickupDef.baseColor);
            var pickupName = Language.GetString(pickupDef.nameToken);
            var itemCount = master.inventory.GetItemCount(pickupDef.itemIndex);

            return $"<color=#{pickupColor}>{pickupName} ({itemCount})</color>";
        }

        // Returns the player color as a hex string w/o the #
        private static string GetPlayerColor(PlayerCharacterMasterController controllerMaster)
        {
            var playerLocation = PlayerCharacterMasterController.instances.IndexOf(controllerMaster);
            return PlayerColors[playerLocation % 8];
        }

        private static string CharacterNameWithColorFormatter(PlayerCharacterMasterController playerCharacterMasterController)
        {
            return $"<color=#{GetPlayerColor(playerCharacterMasterController)}>{playerCharacterMasterController.GetDisplayName()}</color>";
        }

        private static List<PlayerCharacterMasterController> GetEligiblePlayers(CharacterBody body)
        {
            var eligiblePlayers = new List<PlayerCharacterMasterController>();

            foreach (var playerCharacterMasterController in PlayerCharacterMasterController.instances)
            {
                var master = playerCharacterMasterController.master;

                // If they don't have a body or are the one who picked up the item, go to the next person
                if (!master.inventory || master.GetBody() == body)
                {
                    continue;
                }

                // If the player is alive, add one to eligiblePlayers
                if (!master.IsDeadAndOutOfLivesServer() || ShareSuite.DeadPlayersGetItems.Value)
                {
                    eligiblePlayers.Add(playerCharacterMasterController);
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