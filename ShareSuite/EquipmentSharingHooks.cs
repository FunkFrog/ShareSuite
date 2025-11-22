using System.Collections.Generic;
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
            On.RoR2.GenericPickupController.AttemptGrant -= OnGrantEquipment;
        }

        public static void Hook()
        {
            On.RoR2.GenericPickupController.AttemptGrant += OnGrantEquipment;
        }

        private static void OnGrantEquipment(On.RoR2.GenericPickupController.orig_AttemptGrant orig,
            GenericPickupController self, CharacterBody body)
        {
            #region Sharedequipment


            // If equipment sharing is disabled, or we're not in multiplayer, or we're not in an server, run the original method
            if (!ShareSuite.EquipmentShared.Value || !GeneralHooks.IsMultiplayer() || !NetworkServer.active)
            {
                orig(self, body);
                return;
            }

            // Get the old and new equipment's index
            var oldEquip = body.inventory.currentEquipmentIndex;
            var oldEquipPickupIndex = GetPickupIndex(oldEquip);
            var newEquip = PickupCatalog.GetPickupDef(self.pickupIndex).equipmentIndex;

            // If the new item is not equipment, return
            if (IsEquipment(newEquip) == false)
            {
                return;
            }

            // Send the pickup message
            ChatHandler.SendPickupMessage(body.master, new UniquePickup(self.pickupIndex));

            // Give the equipment to the picker 
            body.inventory.SetEquipmentIndex(newEquip);

            // Destroy the object
            Object.Destroy(self.gameObject);

            // If the old equipment was not shared and the new one is, drop the blacklisted equipment and any other
            // shared equipment that the other players have
            if (!EquipmentShared(oldEquip) && EquipmentShared(newEquip))
            {
                CreateDropletIfExists(oldEquipPickupIndex, self.transform.position);
                DropAllOtherSharedEquipment(self, body, oldEquip);
            }
            // If the old equipment was shared and the new one isn't, but the picker is the only one alive with the
            // shared equipment, drop it on the ground and return
            else if (EquipmentShared(oldEquip) && !EquipmentShared(newEquip) &&
                     GetLivingPlayersWithEquipment(oldEquip) < 1
                     || !EquipmentShared(oldEquip) && !EquipmentShared(newEquip))
            {
                CreateDropletIfExists(oldEquipPickupIndex, self.transform.position);
                return;
            }    
            // If the new equip is shared, create a droplet of the old one.
            else if (EquipmentShared(newEquip))
                CreateDropletIfExists(oldEquipPickupIndex, self.transform.position);
            // If the equipment they're picking up is not shared and someone else is alive with the shared equipment,
            // return
            else return;

            // Loop over everyone who has an inventory and isn't the picker
            foreach (var player in PlayerCharacterMasterController.instances.Select(p => p.master)
                .Where(p => p.inventory && p != body.master))
            {
                var playerInventory = player.inventory;
                var playerOrigEquipment = playerInventory.currentEquipmentIndex;

                // If the player currently has an equipment that's blacklisted
                if (!EquipmentShared(playerOrigEquipment))
                {
                    // And the config option is set so that they will drop the item when shared
                    if (!ShareSuite.DropBlacklistedEquipmentOnShare.Value)
                    {
                        continue;
                    }

                    // Create a droplet of their current blacklisted equipment on the ground
                    var transform = player.GetBodyObject().transform;
                    var pickupIndex = PickupCatalog.FindPickupIndex(playerOrigEquipment);
                    PickupDropletController.CreatePickupDroplet(pickupIndex, transform.position,
                        transform.forward * 20f);
                }

                // Give the player the new equipment
                playerInventory.SetEquipmentIndex(newEquip, true);

                // Sync the equipment if they're playing MUL-T
                SyncToolbotEquip(player, ref newEquip);
            }

            #endregion
        }

        private static void DropAllOtherSharedEquipment(GenericPickupController self, CharacterBody body,
            EquipmentIndex originalEquip)
        {
            var droppedEquipment = new List<EquipmentIndex>();
            foreach (var player in PlayerCharacterMasterController.instances.Select(p => p.master)
                .Where(p => p.inventory && p != body.master))
            {
                var playerEquipment = player.inventory.GetEquipmentIndex();
                if (!EquipmentShared(playerEquipment) || droppedEquipment.Contains(playerEquipment) ||
                    playerEquipment == originalEquip) continue;
                CreateDropletIfExists(GetPickupIndex(playerEquipment), self.transform.position);
                droppedEquipment.Add(playerEquipment);
            }
        }

        private static PickupIndex GetPickupIndex(EquipmentIndex originalEquip)
        {
            return originalEquip != EquipmentIndex.None
                ? PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(originalEquip)).pickupIndex
                : PickupIndex.none;
        }

        public static void RemoveAllUnBlacklistedEquipment()
        {
            foreach (var player in PlayerCharacterMasterController.instances.Select(p => p.master)
                .Where(p => p.inventory))
            {
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

        private static void CreateDropletIfExists(PickupIndex pickupIndex, Vector3 position)
        {
            if (pickupIndex != PickupIndex.none)
            {
                PickupDropletController.CreatePickupDroplet(pickupIndex, position,
                    new Vector3(
                        Random.Range(-200f, 200f), 50f, Random.Range(-200f, 200f)));
            }
        }

        private static int GetLivingPlayersWithEquipment(EquipmentIndex originalEquip)
        {
            return PlayerCharacterMasterController.instances.Select(p => p.master)
                .Where(p => p.inventory && !ItemSharingHooks.IsDeadAndNotADrone(p))
                .Count(master => master.inventory.currentEquipmentIndex == originalEquip);
        }

        /// <summary>
        /// This function is currently ineffective, but may be later extended to quickly set a validator
        /// on equipments to narrow them down to a set of ranges beyond just blacklisting.
        /// </summary>
        /// <param name="pickup">Takes a PickupIndex that's a valid equipment.</param>
        /// <returns>True if the given PickupIndex validates, otherwise false.</returns>
        private static bool EquipmentShared(EquipmentIndex pickup)
        {
            if (pickup == EquipmentIndex.None) return true;
            return !Blacklist.HasEquipment(pickup);
        }

        private static bool IsEquipment(EquipmentIndex index)
        {
            return EquipmentCatalog.allEquipment.Contains(index);
        }
    }
}