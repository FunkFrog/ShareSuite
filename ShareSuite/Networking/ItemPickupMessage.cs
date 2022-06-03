using RoR2;
using UnityEngine.Networking;

namespace ShareSuite.Networking
{
    public class ItemPickupMessage: MessageBase
    {
        public PickupIndex PickupIndex { get; private set; }

        // ReSharper disable once UnusedMember.Global
        public ItemPickupMessage()
        {

        }

        public ItemPickupMessage(PickupIndex pickupIndex)
        {
            PickupIndex = pickupIndex;
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(PickupIndex);
        }

        public override void Deserialize(NetworkReader reader)
        {
            PickupIndex = reader.ReadPickupIndex();
        }
    }
}
