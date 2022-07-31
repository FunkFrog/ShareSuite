using System.Linq;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace ShareSuite.Networking
{
    public static class NetworkHandler
    {
        public static void RegisterHandlers()
        {
            var client = NetworkManager.singleton?.client;

            if (client == null || client.handlers.ContainsKey(ShareSuite.NetworkMessageType.Value))
            {
                return;
            }

            client.RegisterHandler(ShareSuite.NetworkMessageType.Value, ItemPickupHandler);
        }

        public static void SendItemPickupMessage(int connectionId, PickupIndex pickupIndex)
        {
            NetworkServer.SendToClient(connectionId, ShareSuite.NetworkMessageType.Value, new ItemPickupMessage(pickupIndex));
        }

        private static void ItemPickupHandler(NetworkMessage networkMessage)
        {
            var itemPickupMessage = networkMessage.ReadMessage<ItemPickupMessage>();
            var localPlayer = PlayerCharacterMasterController.instances.FirstOrDefault(x => x.networkUser.isLocalPlayer);

            if (localPlayer == null || !itemPickupMessage.PickupIndex.isValid)
            {
                return;
            }

            ItemSharingHooks.HandleRichMessageUnlockAndNotification(localPlayer.master, itemPickupMessage.PickupIndex);
        }
    }
}
