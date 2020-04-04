using RoR2;
using System.Collections.Generic;
using System.Linq;

namespace ShareSuite
{
    public static class Blacklist
    {
        private static ItemMask _cachedAvailableItems;

        public static ItemMask _items = ItemMask.none;
        public static EquipmentMask _equipment = EquipmentMask.none;
        private static readonly List<PickupIndex> _availableTier1DropList = new List<PickupIndex>();
        private static readonly List<PickupIndex> _availableTier2DropList = new List<PickupIndex>();
        private static readonly List<PickupIndex> _availableTier3DropList = new List<PickupIndex>();
        private static readonly List<PickupIndex> _availableLunarDropList = new List<PickupIndex>();
        private static readonly List<PickupIndex> _availableBossDropList = new List<PickupIndex>();

        public static List<PickupIndex> AvailableTier1DropList => Get(_availableTier1DropList);
        public static List<PickupIndex> AvailableTier2DropList => Get(_availableTier2DropList);
        public static List<PickupIndex> AvailableTier3DropList => Get(_availableTier3DropList);
        public static List<PickupIndex> AvailableLunarDropList => Get(_availableLunarDropList);
        public static List<PickupIndex> AvailableBossDropList => Get(_availableBossDropList);

        private static void LoadBlackListItems()
        {
            _items = ItemMask.none;
            foreach (var piece in ShareSuite.ItemBlacklist.Value.Split(','))
                if (int.TryParse(piece.Trim(), out var itemIndex))
                    _items.AddItem((ItemIndex) itemIndex);
        }

        private static void LoadBlackListEquipment()
        {
            _equipment = EquipmentMask.none;
            foreach (var piece in ShareSuite.EquipmentBlacklist.Value.Split(','))
                if (int.TryParse(piece.Trim(), out var equipmentIndex))
                    _equipment.AddEquipment((EquipmentIndex) equipmentIndex);
        }

        public static bool HasItem(ItemIndex itemIndex)
        {
            ValidateItemCache();
            return _items.HasItem(itemIndex);
        }

        public static bool HasEquipment(EquipmentIndex equipmentIndex)
        {
            ValidateItemCache();
            return _equipment.HasEquipment(equipmentIndex);
        }

        public static void Recalculate() => _cachedAvailableItems = ItemMask.none;

        private static void ValidateItemCache()
        {
            if (Run.instance == null)
            {
                Recalculate();
                return;
            }

            if (ItemMaskEquals(_cachedAvailableItems, Run.instance.availableItems))
                return;

            _cachedAvailableItems = Run.instance.availableItems;

            // Available items have changed; recalculate available items minus blacklists

            LoadBlackListItems();
            LoadBlackListEquipment();

            var pairs = new[] {
                (_availableTier1DropList, Run.instance.availableTier1DropList),
                (_availableTier2DropList, Run.instance.availableTier2DropList),
                (_availableTier3DropList, Run.instance.availableTier3DropList),
                (_availableLunarDropList, Run.instance.availableLunarDropList),
                (_availableBossDropList , Run.instance.availableBossDropList ),
            };
            foreach (var (availMinusBlack, source) in pairs)
            {
                availMinusBlack.Clear();
                availMinusBlack.AddRange(source.Where(pickupIndex =>
                {
                    var pickupDef = PickupCatalog.GetPickupDef(pickupIndex);
                    return !_items.HasItem(pickupDef.itemIndex);
                }));
            }
        }

        // Util

        private static List<PickupIndex> Get(List<PickupIndex> list)
        {
            ValidateItemCache();
            return list;
        }

        private static bool ItemMaskEquals(ItemMask first, ItemMask second)
            => first.a == second.a && first.b == second.b;
    }
}
