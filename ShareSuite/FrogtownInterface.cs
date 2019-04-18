using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using RoR2;
using UnityEngine;
using UnityEngine.Events;

namespace ShareSuite
{
    public static class FrogtownInterface
    {
        private static ConfigFile _config;
        private static List<object> availableSettings = new List<object>();
        private static Vector2 _scrollPos;
        private static HashSet<int> _bannedItems;

        public static void Init(ConfigFile config)
        {
            //Soft dependency on mod loader
            _config = config;
            Type modDetailsType = null;
            Type frogtownSharedType = null;
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!a.FullName.StartsWith("FrogtownShared,")) continue;
                var allTypes = a.GetTypes();
                foreach (var t in allTypes)
                {
                    switch (t.Name)
                    {
                        case "FrogtownModDetails":
                            modDetailsType = t;
                            break;
                        case "FrogtownShared":
                            frogtownSharedType = t;
                            break;
                    }
                }

                break;
            }

            try
            {
                if (modDetailsType == null || frogtownSharedType == null) return;
                //Will be set back to true by the manager when it initializes
                ShareSuite.WrapModIsEnabled.Value = false;

                var obj = Activator.CreateInstance(modDetailsType, "com.funkfrog_sipondo.sharesuite");
                obj.SetFieldValue("githubAuthor", "FunkFrog");
                obj.SetFieldValue("githubRepo", "RoR2SharedItems");
                obj.SetFieldValue("description",
                    "Multiplayer RoR2 games should be quick wacky fun, but are often plagued by loot and chest sniping. This mod aims to fix that!");
                obj.SetFieldValue("OnGUI", new UnityAction(() => { OnSettingsGui(); }));
                obj.SetFieldValue("afterToggle", new UnityAction(() =>
                {
                    ShareSuite.WrapModIsEnabled.Value = obj.GetPropertyValue<bool>("enabled");
                    config.Save();
                }));

                var register = frogtownSharedType.GetMethod("RegisterMod");
                if (register != null) register.Invoke(null, new[] {obj});
                InitSettings();
            }
            catch (Exception e)
            {
                Debug.Log("Failed to initialize mod manager features");
                Debug.Log(e.StackTrace);
            }
        }

        private static void InitSettings()
        {
            //Build list of settings that can be controlled in the UI
            availableSettings.Add(ShareSuite.WrapMoneyIsShared);
            availableSettings.Add(ShareSuite.WrapWhiteItemsShared);
            availableSettings.Add(ShareSuite.WrapGreenItemsShared);
            availableSettings.Add(ShareSuite.WrapRedItemsShared);
            availableSettings.Add(ShareSuite.WrapLunarItemsShared);
            availableSettings.Add(ShareSuite.WrapBossItemsShared);
            availableSettings.Add(ShareSuite.WrapQueensGlandsShared);
            availableSettings.Add(ShareSuite.WrapPrinterCauldronFixEnabled);
            availableSettings.Add(ShareSuite.WrapOverridePlayerScalingEnabled);
            availableSettings.Add(ShareSuite.WrapOverrideBossLootScalingEnabled);
            availableSettings.Add(ShareSuite.WrapDeadPlayersGetItems);
            availableSettings.Add(ShareSuite.WrapMoneyScalar);
            availableSettings.Add(ShareSuite.WrapInteractablesCredit);
            availableSettings.Add(ShareSuite.WrapBossLootCredit);
            availableSettings.Add(ShareSuite.WrapItemBlacklist);

            _bannedItems = ShareSuite.GetItemBlackList();
        }

        private static void OnSettingsGui()
        {
            foreach (var rawSetting in availableSettings)
            {
                switch (rawSetting)
                {
                    case ConfigWrapper<bool> boolSetting:
                    {
                        //check box style settings
                        var newValue = GUILayout.Toggle(boolSetting.Value,
                            new GUIContent("  " + boolSetting.Definition.Key, boolSetting.Definition.Description));
                        if (newValue != boolSetting.Value)
                        {
                            boolSetting.Value = newValue;
                            _config.Save();
                        }

                        break;
                    }
                    case ConfigWrapper<int> intSetting:
                    {
                        //numeric settings with a slider
                        GUILayout.Label(new GUIContent(intSetting.Definition.Key + ": " + intSetting.Value,
                            intSetting.Definition.Description));
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("1", GUILayout.ExpandWidth(false));
                        double rawSelection = GUILayout.HorizontalSlider(intSetting.Value, 1, 10, GUILayout.Width(200));
                        var newValue = (int) Math.Round(rawSelection);
                        if (newValue != intSetting.Value)
                        {
                            intSetting.Value = newValue;
                            _config.Save();
                        }

                        GUILayout.Label("10", GUILayout.ExpandWidth(false));
                        GUILayout.EndHorizontal();
                        break;
                    }
                    case ConfigWrapper<string> itemSetting:
                    {
                        //banned item setting
                        GUILayout.Label(new GUIContent(itemSetting.Definition.Key, itemSetting.Definition.Description));
                        _scrollPos = GUILayout.BeginScrollView(_scrollPos, GUILayout.Height(90));
                        GUILayout.BeginHorizontal();
                        foreach (var itemIndex in ItemCatalog.allItems)
                        {
                            var itemDef = ItemCatalog.GetItemDef(itemIndex);
                            if (itemDef.tier == ItemTier.NoTier)
                            {
                                continue;
                            }

                            var name = Language.GetString(itemDef.nameToken);
                            var isBanned = _bannedItems.Contains((int) itemDef.itemIndex);
                            var oldcolor = GUI.backgroundColor;
                            GUI.backgroundColor = isBanned ? Color.red : Color.white;
                            var newIsBanned = GUILayout.Toggle(isBanned,
                                new GUIContent(itemDef.pickupIconTexture, name), GUILayout.Width(64),
                                GUILayout.Height(64));
                            GUI.backgroundColor = oldcolor;
                            if (isBanned == newIsBanned) continue;
                            if (newIsBanned)
                            {
                                _bannedItems.Add((int) itemDef.itemIndex);
                            }
                            else
                            {
                                _bannedItems.Remove((int) itemDef.itemIndex);
                            }

                            itemSetting.Value = SetToStringList(_bannedItems);
                            _config.Save();
                        }

                        GUILayout.EndHorizontal();
                        GUILayout.EndScrollView();
                        break;
                    }
                }
            }
        }

        private static string SetToStringList(IEnumerable<int> set)
        {
            var list = "";
            foreach (int value in set)
            {
                if (list.Length > 0)
                {
                    list += ",";
                }

                list += value.ToString();
            }

            return list;
        }
    }
}