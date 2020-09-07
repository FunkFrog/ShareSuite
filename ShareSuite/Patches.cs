using HarmonyLib;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ShareSuite
{
    public class Patches
    {
        [HarmonyPatch(typeof(Util), "GetExpAdjustedDropChancePercent", new Type[] { typeof(float), typeof(GameObject) })]
        [HarmonyPrefix]
        public static bool Patch_GetExpAdjustedDropChancePercent(ref float baseChancePercent)
        {
            if (ShareSuite.SacrificeFixEnabled.Value)
            {
                baseChancePercent /= Math.Min(4, PlayerCharacterMasterController.instances.Count);
            }

            return true;
        }
    }
}
