using RoR2;
using UnityEngine.Networking;

namespace ShareSuite.Networking
{
    public static class NetworkHandler
    {
        private static void RegisterHandlers()
        {
            var client = NetworkManager.singleton.client;

            client.RegisterHandler(ShareSuite.NetworkMessageType.Value, ItemPickupHandler);
        }

        public static void Setup()
        {
            RegisterHandlers();
        }

        public static void SendItemPickupMessage(int connectionId, PickupIndex pickupIndex)
        {
            NetworkServer.SendToClient(connectionId, ShareSuite.NetworkMessageType.Value, new ItemPickupMessage(pickupIndex));
        }

        private static void ItemPickupHandler(NetworkMessage networkMessage)
        {
            var itemPickupMessage = networkMessage.ReadMessage<ItemPickupMessage>();
            var localPlayer = PlayerCharacterMasterController.instances[0];

            if (!itemPickupMessage.PickupIndex.isValid)
            {
                return;
            }

            ItemSharingHooks.HandleRichMessageUnlockAndNotification(localPlayer.master, itemPickupMessage.PickupIndex);
        }
    }
}
