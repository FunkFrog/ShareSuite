using RoR2;

namespace ShareSuite.Extensions
{
    public static class ItemDefExtensions
    {
        public static bool IsVoid(this ItemDef self)
        {
            return self.tier == ItemTier.VoidTier1 ||
                   self.tier == ItemTier.VoidTier2 ||
                   self.tier == ItemTier.VoidTier3 ||
                   self.tier == ItemTier.VoidBoss;
        }

        public static PickupDef GetPickupDef(this ItemDef self)
        {
            return PickupCatalog.GetPickupDef(PickupCatalog.FindPickupIndex(self.itemIndex));
        }
    }
}
