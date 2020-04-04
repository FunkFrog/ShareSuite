using System.Linq;
using RoR2;
using UnityEngine.Networking;

namespace ShareSuite
{
    public static class EquipmentSharingHooks
    {
        public static void UnHook()
        {
            On.RoR2.GenericPickupController.GrantEquipment -= OnGrantEquipment;
        }
        public static void Hook()
        {
            On.RoR2.GenericPickupController.GrantEquipment += OnGrantEquipment;
        }

        private static void OnGrantEquipment(On.RoR2.GenericPickupController.orig_GrantEquipment orig, GenericPickupController self, CharacterBody body, Inventory inventory)
        {
            #region Sharedequipment

            var equip = PickupCatalog.GetPickupDef(self.pickupIndex).equipmentIndex;

            if (!BlackList.HasEquipment(equip)
                && NetworkServer.active
                && IsValidEquipmentPickup(self.pickupIndex)
                && GeneralHooks.IsMultiplayer())
                foreach (var player in PlayerCharacterMasterController.instances.Select(p => p.master)
                    .Where(p => !p.IsDeadAndOutOfLivesServer() || ShareSuite.DeadPlayersGetItems.Value))
                {
                    SyncToolbotEquip(player, ref equip);

                    // Sync Mul-T Equipment, but perform primary equipment pickup only for clients
                    if (player.inventory == inventory) continue;

                    player.inventory.SetEquipmentIndex(equip);
                    self.NetworkpickupIndex = PickupCatalog.FindPickupIndex(equip);
                }

            orig(self, body, inventory);

            #endregion
        }

        private static void SetEquipmentIndex(Inventory self, EquipmentIndex newEquipmentIndex, uint slot)
        {
            if (!NetworkServer.active) return;
            if (self.currentEquipmentIndex == newEquipmentIndex) return;
            var equipment = self.GetEquipment(0U);
            var charges = equipment.charges;
            if (equipment.equipmentIndex == EquipmentIndex.None) charges = 1;
            self.SetEquipment(new EquipmentState(newEquipmentIndex, equipment.chargeFinishTime, charges), slot);
        }

        private static void SyncToolbotEquip(CharacterMaster characterMaster, ref EquipmentIndex equip)
        {
            if (characterMaster.bodyPrefab.name != "ToolbotBody") return;
            SetEquipmentIndex(characterMaster.inventory, equip,
                (uint) (characterMaster.inventory.activeEquipmentSlot + 1) % 2);
        }

        /// <summary>
        /// This function is currently ineffective, but may be later extended to quickly set a validator
        /// on equipments to narrow them down to a set of ranges beyond just blacklisting.
        /// </summary>
        /// <param name="pickup">Takes a PickupIndex that's a valid equipment.</param>
        /// <returns>True if the given PickupIndex validates, otherwise false.</returns>
        private static bool IsValidEquipmentPickup(PickupIndex pickup)
        {
            var equip = PickupCatalog.GetPickupDef(pickup).equipmentIndex;
            return IsEquipment(equip) && ShareSuite.EquipmentShared.Value;
        }

        public static bool IsEquipment(EquipmentIndex index)
        {
            return EquipmentCatalog.allEquipment.Contains(index);
        }
    }
}