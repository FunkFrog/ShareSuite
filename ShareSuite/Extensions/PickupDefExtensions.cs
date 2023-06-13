using RoR2;

namespace ShareSuite.Extensions
{
    public static class PickupDefExtensions
    {
        public static bool IsVoid(this PickupDef self)
        {
            return self.itemTier == ItemTier.VoidTier1 ||
                   self.itemTier == ItemTier.VoidTier2 ||
                   self.itemTier == ItemTier.VoidTier3 ||
                   self.itemTier == ItemTier.VoidBoss;
        }
    }
}
