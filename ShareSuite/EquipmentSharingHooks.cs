using System.Linq;
using RoR2;
using UnityEngine;
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

        private static void OnGrantEquipment(On.RoR2.GenericPickupController.orig_GrantEquipment orig,
            GenericPickupController self, CharacterBody body, Inventory inventory)
        {
            #region Sharedequipment

            var equip = PickupCatalog.GetPickupDef(self.pickupIndex).equipmentIndex;

            if (NetworkServer.active && IsValidEquipmentPickup(self.pickupIndex) && GeneralHooks.IsMultiplayer())
                if (!Blacklist.HasEquipment(equip))
                {
                    Debug.Log("item isn't on the blacklist so giving to everyone");
                    foreach (var player in PlayerCharacterMasterController.instances.Select(p => p.master)
                        .Where(p => !p.IsDeadAndOutOfLivesServer() || ShareSuite.DeadPlayersGetItems.Value))
                    {
                        // Sync Mul-T Equipment, but perform primary equipment pickup only for clients
                        if (!player.inventory) continue;

                        GivePlayerEquipment(self, player, equip);

                        SyncToolbotEquip(player, ref equip);
                    }
                    
                    orig(self, body, inventory);
                    return;
                }
                else
                {
                    body.master.inventory.SetEquipmentIndex(equip);
                    Object.Destroy(self.gameObject);
                    return;
                }

            orig(self, body, inventory);

            #endregion
        }

        private static void GivePlayerEquipment(GenericPickupController self,
            CharacterMaster player, EquipmentIndex equip)
        {
            var inventory = player.inventory;

            if (Blacklist.HasEquipment(inventory.currentEquipmentIndex))
            {
                if (!ShareSuite.DropBlacklistedEquipmentOnShare.Value) return;
                var transform = player.GetBodyObject().transform;
                var pickupIndex = PickupCatalog.FindPickupIndex(inventory.currentEquipmentIndex);

                PickupDropletController.CreatePickupDroplet(pickupIndex, transform.position,
                    transform.forward * 20f);
            }

            inventory.SetEquipmentIndex(equip);
            self.NetworkpickupIndex = PickupCatalog.FindPickupIndex(equip);
        }

        public static void RemoveAllUnBlacklistedEquipment()
        {
            foreach (var player in PlayerCharacterMasterController.instances.Select(p => p.master)
                .Where(p => ShareSuite.DeadPlayersGetItems.Value))
            {
                if (!player.inventory) return;

                if (!Blacklist.HasEquipment(player.inventory.currentEquipmentIndex))
                {
                    player.inventory.SetEquipmentIndex(EquipmentIndex.None);
                }
            }
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

        private static bool IsEquipment(EquipmentIndex index)
        {
            return EquipmentCatalog.allEquipment.Contains(index);
        }
    }
}