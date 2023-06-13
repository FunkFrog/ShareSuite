using System.Collections.Generic;
using System.Linq;
using R2API.MiscHelpers;
using RoR2;

namespace ShareSuite.Helpers
{
    public static class VoidItemsHelper
    {
        /// <summary>
        /// Dictionary of each void item's corresponding base item(s).
        /// </summary>
        public static readonly Dictionary<ItemDef, List<ItemDef>> BaseItemsCorrespondences = new Dictionary<ItemDef, List<ItemDef>>
        {
            { DLC1Content.Items.BearVoid, new List<ItemDef> {RoR2Content.Items.Bear} },
            { DLC1Content.Items.BleedOnHitVoid, new List<ItemDef> { RoR2Content.Items.BleedOnHit } },
            { DLC1Content.Items.ChainLightningVoid, new List<ItemDef> { RoR2Content.Items.ChainLightning } },
            { DLC1Content.Items.CloverVoid, new List<ItemDef> { RoR2Content.Items.Clover } },
            { DLC1Content.Items.CritGlassesVoid, new List<ItemDef> { RoR2Content.Items.CritGlasses } },
            { DLC1Content.Items.EquipmentMagazineVoid, new List<ItemDef> { RoR2Content.Items.EquipmentMagazine } },
            { DLC1Content.Items.ExplodeOnDeathVoid, new List<ItemDef> { RoR2Content.Items.ExplodeOnDeath } },
            { DLC1Content.Items.ExtraLifeVoid, new List<ItemDef> { RoR2Content.Items.ExtraLife } },
            { DLC1Content.Items.MissileVoid, new List<ItemDef> { RoR2Content.Items.Missile } },
            { DLC1Content.Items.MushroomVoid, new List<ItemDef> { RoR2Content.Items.Mushroom } },
            { DLC1Content.Items.SlowOnHitVoid, new List<ItemDef> { RoR2Content.Items.SlowOnHit } },
            { DLC1Content.Items.TreasureCacheVoid, new List<ItemDef> { RoR2Content.Items.TreasureCache } },
            { DLC1Content.Items.ElementalRingVoid, new List<ItemDef> { RoR2Content.Items.FireRing, RoR2Content.Items.IceRing } },
            { DLC1Content.Items.VoidMegaCrabItem, ItemCatalog.allItemDefs.Where(x => x.tier == ItemTier.Boss && x.nameToken != "ArtifactKey").ToList() }
        };

        /// <summary>
        /// Dictionary of each item's existing void version.
        /// </summary>
        public static Dictionary<ItemDef, ItemDef> VoidItemCorrespondences
        {
            get
            {
                var reverseDict = new Dictionary<ItemDef, ItemDef>();

                foreach (var (key, values) in BaseItemsCorrespondences)
                {
                    foreach (var value in values)
                    {
                        reverseDict.Add(value, key);
                    }
                }

                return reverseDict;
            }
        }
    }
}
