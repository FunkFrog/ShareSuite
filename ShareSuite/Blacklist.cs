using RoR2;
using System.Collections.Generic;
using System.Linq;

namespace ShareSuite
{
    public static class Blacklist
    {
        private static ItemMask _cachedAvailableItems = new ItemMask();

        private static ItemMask _items = new ItemMask();
        private static EquipmentMask _equipment = new EquipmentMask();
        // ReSharper disable InconsistentNaming
        private static readonly List<PickupIndex> _availableTier1DropList = new List<PickupIndex>();
        private static readonly List<PickupIndex> _availableTier2DropList = new List<PickupIndex>();
        private static readonly List<PickupIndex> _availableTier3DropList = new List<PickupIndex>();
        private static readonly List<PickupIndex> _availableLunarDropList = new List<PickupIndex>();
        private static readonly List<PickupIndex> _availableBossDropList = new List<PickupIndex>();
        private static readonly List<PickupIndex> _availableVoidDropList = new List<PickupIndex>();
        // ReSharper enable InconsistentNaming

        public static List<PickupIndex> AvailableTier1DropList => Get(_availableTier1DropList);
        public static List<PickupIndex> AvailableTier2DropList => Get(_availableTier2DropList);
        public static List<PickupIndex> AvailableTier3DropList => Get(_availableTier3DropList);
        public static List<PickupIndex> AvailableLunarDropList => Get(_availableLunarDropList);
        public static List<PickupIndex> AvailableBossDropList => Get(_availableBossDropList);
        public static List<PickupIndex> AvailableVoidDropList => Get(_availableVoidDropList);

        private static void LoadBlackListItems()
        {
            _items = new ItemMask();
            foreach (var piece in ShareSuite.ItemBlacklist.Value.Split(','))
            {
                // if (int.TryParse(piece.Trim(), out var itemIndex))
                //     _items.Add((ItemIndex) itemIndex);
                var item = ItemCatalog.FindItemIndex(piece);
                if (item == ItemIndex.None) continue;

                _items.Add(item);
            }
        }

        private static void LoadBlackListEquipment()
        {
            _equipment = new EquipmentMask();
            foreach (var piece in ShareSuite.EquipmentBlacklist.Value.Split(','))
            {
                // if (int.TryParse(piece.Trim(), out var equipmentIndex))
                //     _equipment.Add((EquipmentIndex) equipmentIndex);
                var equip = EquipmentCatalog.FindEquipmentIndex(piece);
                if (equip == EquipmentIndex.None) continue;

                _equipment.Add(equip);
            }
        }

        public static bool HasItem(ItemIndex itemIndex)
        {
            ValidateItemCache();
            return _items.Contains(itemIndex);
        }

        public static bool HasEquipment(EquipmentIndex equipmentIndex)
        {
            ValidateItemCache();
            return _equipment.Contains(equipmentIndex);
        }

        public static void Recalculate() => _cachedAvailableItems = new ItemMask();

        private static void ValidateItemCache()
        {
            if (Run.instance == null)
            {
                Recalculate();
                return;
            }
            
            if (_cachedAvailableItems.Equals(Run.instance.availableItems))
                return;

            _cachedAvailableItems = Run.instance.availableItems;

            // Available items have changed; recalculate available items minus blacklists
            
            LoadBlackListItems();
            LoadBlackListEquipment();

            var combinedAvailableVoidDropList = new List<PickupIndex>()
                .Concat(Run.instance.availableVoidTier1DropList)
                .Concat(Run.instance.availableVoidTier2DropList)
                .Concat(Run.instance.availableVoidTier3DropList)
                .Concat(Run.instance.availableVoidBossDropList);

            var pairs = new[] {
                (_availableTier1DropList, Run.instance.availableTier1DropList),
                (_availableTier2DropList, Run.instance.availableTier2DropList),
                (_availableTier3DropList, Run.instance.availableTier3DropList),
                (_availableLunarDropList, Run.instance.availableLunarItemDropList),
                (_availableBossDropList , Run.instance.availableBossDropList),
                (_availableVoidDropList , combinedAvailableVoidDropList)
            };
            foreach (var (availMinusBlack, source) in pairs)
            {
                availMinusBlack.Clear();
                availMinusBlack.AddRange(source.Where(pickupIndex =>
                {
                    var pickupDef = PickupCatalog.GetPickupDef(pickupIndex);
                    return pickupDef != null && !_items.Contains(pickupDef.itemIndex);
                }));
            }
        }

        // Util

        private static List<PickupIndex> Get(List<PickupIndex> list)
        {
            ValidateItemCache();
            return list;
        }
    }
}
