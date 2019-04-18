using BepInEx.Configuration;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ShareSuite
{
    public class FrogtownInterface
    {
        private static ConfigFile config;
        private static List<object> availableSettings = new List<object>();
        private static Vector2 scrollPos;
        private static HashSet<int> bannedItems;

        public static void Init(ConfigFile config)
        {
            //Soft dependency on mod loader
            FrogtownInterface.config = config;
            Type modDetailsType = null;
            Type frogtownSharedType = null;
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (a.FullName.StartsWith("FrogtownShared,"))
                {
                    var allTypes = a.GetTypes();
                    foreach (Type t in allTypes)
                    {
                        if (t.Name == "FrogtownModDetails")
                        {
                            modDetailsType = t;
                        }
                        if (t.Name == "FrogtownShared")
                        {
                            frogtownSharedType = t;
                        }
                    }
                    break;
                }
            }

            try
            {
                if (modDetailsType != null && frogtownSharedType != null)
                {
                    //Will be set back to true by the manager when it initializes
                    ShareSuite.WrapModIsEnabled.Value = false;

                    var obj = Activator.CreateInstance(modDetailsType, "com.funkfrog_sipondo.sharesuite");
                    obj.SetFieldValue("githubAuthor", "FunkFrog");
                    obj.SetFieldValue("githubRepo", "RoR2SharedItems");
                    obj.SetFieldValue("description", "Multiplayer RoR2 games should be quick wacky fun, but are often plagued by loot and chest sniping. This mod aims to fix that!");
                    obj.SetFieldValue("OnGUI", new UnityAction(() =>
                    {
                        OnSettingsGUI();
                    }));
                    obj.SetFieldValue("afterToggle", new UnityAction(() =>
                    {
                        ShareSuite.WrapModIsEnabled.Value = obj.GetPropertyValue<bool>("enabled");
                        config.Save();
                    }));

                    var register = frogtownSharedType.GetMethod("RegisterMod");
                    register.Invoke(null, new object[] { obj });
                    InitSettings();
                }
            }catch(Exception e)
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

            bannedItems = ShareSuite.GetItemBlackList();
        }

        public static void OnSettingsGUI()
        {
            for(int i = 0; i < availableSettings.Count; i++)
            {
                var rawSetting = availableSettings[i];
                var boolSetting = rawSetting as ConfigWrapper<bool>;
                if (boolSetting != null)
                {
                    //check box style settings
                    bool newValue = GUILayout.Toggle(boolSetting.Value, new GUIContent("  " + boolSetting.Definition.Key, boolSetting.Definition.Description));
                    if (newValue != boolSetting.Value)
                    {
                        boolSetting.Value = newValue;
                        config.Save();
                    }
                }

                var intSetting = rawSetting as ConfigWrapper<int>;
                if (intSetting != null)
                {
                    //numeric settings with a slider
                    GUILayout.Label(new GUIContent(intSetting.Definition.Key + ": " + intSetting.Value, intSetting.Definition.Description));
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("0", GUILayout.ExpandWidth(false));
                    double rawSelection = GUILayout.HorizontalSlider(intSetting.Value, 0, 32, GUILayout.Width(200));
                    int newValue = (int)Math.Round(rawSelection);
                    if (newValue != intSetting.Value)
                    {
                        intSetting.Value = newValue;
                        config.Save();
                    }
                    GUILayout.Label("32", GUILayout.ExpandWidth(false));
                    GUILayout.EndHorizontal();
                }

                var itemSetting = rawSetting as ConfigWrapper<string>;
                if (itemSetting != null)
                {
                    //banned item setting
                    GUILayout.Label(new GUIContent(itemSetting.Definition.Key, itemSetting.Definition.Description));
                    scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(90));
                    GUILayout.BeginHorizontal();
                    foreach (var itemIndex in ItemCatalog.allItems)
                    {
                        var itemDef = ItemCatalog.GetItemDef(itemIndex);
                        if (itemDef.tier == ItemTier.NoTier)
                        {
                            continue;
                        }

                        var name = Language.GetString(itemDef.nameToken);
                        bool isBanned = bannedItems.Contains((int)itemDef.itemIndex);
                        var oldcolor = GUI.backgroundColor;
                        GUI.backgroundColor = isBanned ? Color.red : Color.white;
                        bool newIsBanned = GUILayout.Toggle(isBanned, new GUIContent(itemDef.pickupIconTexture, name), GUILayout.Width(64), GUILayout.Height(64));
                        GUI.backgroundColor = oldcolor;
                        if (isBanned != newIsBanned)
                        {
                            if (newIsBanned)
                            {
                                bannedItems.Add((int)itemDef.itemIndex);
                            }
                            else
                            {
                                bannedItems.Remove((int)itemDef.itemIndex);
                            }
                            itemSetting.Value = SetToStringList(bannedItems);
                            config.Save();
                        }
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.EndScrollView();
                }
            }
        }

        private static string SetToStringList(HashSet<int> set)
        {
            string list = "";
            foreach(int value in set)
            {
                if(list.Length > 0)
                {
                    list += ",";
                }
                list += value.ToString();
            }
            return list;
        }
    }
}
