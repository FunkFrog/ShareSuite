using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using BepInEx.Configuration;
using RoR2;
using UnityEngine;
using UnityEngine.Events;

namespace ShareSuite
{
    public static class FrogtownInterface
    {
        private static ConfigFile _config;
        private static List<object> _availableSettings;
        private static Vector2 _scrollPos;
        private static Vector2 _scrollPos2;
        private static HashSet<int> _bannedItems;
        private static HashSet<int> _bannedEquipment;
        private static List<ItemIndex> _itemsByRarity;
        private static List<EquipmentIndex> _equipment;

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
                ShareSuite.ModIsEnabled.Value = false;

                var obj = Activator.CreateInstance(modDetailsType, "com.funkfrog_sipondo.sharesuite");
                obj.SetFieldValue("githubAuthor", "FunkFrog");
                obj.SetFieldValue("githubRepo", "RoR2SharedItems");
                obj.SetFieldValue("description",
                    "Multiplayer RoR2 games should be quick wacky fun, but are often plagued by loot and chest sniping. This mod aims to fix that!");
                obj.SetFieldValue("OnGUI", new UnityAction(() => { OnSettingsGui(); }));
                obj.SetFieldValue("afterToggle", new UnityAction(() =>
                {
                    ShareSuite.ModIsEnabled.Value = obj.GetPropertyValue<bool>("enabled");
                    config.Save();
                }));

                var register = frogtownSharedType.GetMethod("RegisterMod");
                if (register != null) register.Invoke(null, new[] {obj});
                InitSettings();
                InitItemList();
                InitEquipmentList();
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
            _availableSettings = new List<object>();

            _availableSettings.Add(ShareSuite.MoneyIsShared);
            _availableSettings.Add(ShareSuite.WhiteItemsShared);
            _availableSettings.Add(ShareSuite.GreenItemsShared);
            _availableSettings.Add(ShareSuite.RedItemsShared);
            _availableSettings.Add(ShareSuite.EquipmentShared);
            _availableSettings.Add(ShareSuite.LunarItemsShared);
            _availableSettings.Add(ShareSuite.BossItemsShared);
            _availableSettings.Add(ShareSuite.QueensGlandsShared);
            _availableSettings.Add(ShareSuite.PrinterCauldronFixEnabled);
            _availableSettings.Add(ShareSuite.DeadPlayersGetItems);
            _availableSettings.Add(ShareSuite.OverridePlayerScalingEnabled);
            _availableSettings.Add(ShareSuite.InteractablesCredit);
            _availableSettings.Add(ShareSuite.OverrideBossLootScalingEnabled);
            _availableSettings.Add(ShareSuite.BossLootCredit);
            _availableSettings.Add(ShareSuite.MoneyScalarEnabled);
            _availableSettings.Add(ShareSuite.MoneyScalar);
            _availableSettings.Add(ShareSuite.ItemBlacklist);
            _availableSettings.Add(ShareSuite.EquipmentBlacklist);

            _bannedItems = ShareSuite.GetItemBlackList();
            _bannedEquipment = ShareSuite.GetEquipmentBlackList();
        }

        private static void InitItemList()
        {
            _itemsByRarity = new List<ItemIndex>();
            foreach (var itemIndex in ItemCatalog.allItems)
            {
                var itemDef = ItemCatalog.GetItemDef(itemIndex);
                if (itemDef.tier == ItemTier.NoTier) continue;
                _itemsByRarity.Add(itemIndex);
            }
            _itemsByRarity.Sort((a, b) =>
            {
                var definitionA = ItemCatalog.GetItemDef(a);
                var definitionB = ItemCatalog.GetItemDef(b);
                return definitionA.tier.CompareTo(definitionB.tier);
            });
        }

        private static void InitEquipmentList()
        {
            _equipment = new List<EquipmentIndex>();
            foreach (var equipmentIndex in EquipmentCatalog.allEquipment)
            {
                var itemDef = EquipmentCatalog.GetEquipmentDef(equipmentIndex);
                if (itemDef.equipmentIndex == EquipmentIndex.None) continue;
                _equipment.Add(equipmentIndex);
            }
            _equipment.Sort((a, b) =>
            {
                var definitionA = EquipmentCatalog.GetEquipmentDef(a);
                var definitionB = EquipmentCatalog.GetEquipmentDef(b);
                return definitionA.equipmentIndex.CompareTo(definitionB.equipmentIndex);
            });
        }

        private static void OnSettingsGui()
        {
            foreach (var rawSetting in _availableSettings)
            {
                switch (rawSetting)
                {
                    case ConfigWrapper<bool> boolSetting:
                    {
                        //check box style settings
                        var newValue = GUILayout.Toggle(boolSetting.Value,
                            new GUIContent(AddSpaces(boolSetting.Definition.Key), boolSetting.Definition.Description));
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
                        GUILayout.Label(new GUIContent(AddSpaces(intSetting.Definition.Key) + ": " + intSetting.Value,
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
                    case ConfigWrapper<string> pickupSetting:
                    {
                        bool isItemBlacklist = !pickupSetting.Definition.Key.ToLower().Contains("equipment");
                        
                        //banned item setting
                        GUILayout.Label(new GUIContent(AddSpaces(pickupSetting.Definition.Key), pickupSetting.Definition.Description));

                        // Temporary switch for item/equipment blacklist
                        if (isItemBlacklist)
                        {
                            _scrollPos = GUILayout.BeginScrollView(_scrollPos, GUILayout.Height(90));
                            GUILayout.BeginHorizontal();
                            foreach (var itemIndex in _itemsByRarity)
                            {
                                var itemDef = ItemCatalog.GetItemDef(itemIndex);
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

                                pickupSetting.Value = SetToStringList(_bannedItems);
                                _config.Save();
                            }
                        }
                        else
                        {
                            _scrollPos2 = GUILayout.BeginScrollView(_scrollPos2, GUILayout.Height(90));
                            GUILayout.BeginHorizontal();
                            foreach (var equipmentIndex in _equipment)
                            {
                                var equipmentDef = EquipmentCatalog.GetEquipmentDef(equipmentIndex);
                                var name = Language.GetString(equipmentDef.nameToken);
                                var isBanned = _bannedEquipment.Contains((int) equipmentDef.equipmentIndex);
                                var oldcolor = GUI.backgroundColor;
                                GUI.backgroundColor = isBanned ? Color.red : Color.white;
                                var newIsBanned = GUILayout.Toggle(isBanned,
                                    new GUIContent(equipmentDef.pickupIconTexture, name), GUILayout.Width(64),
                                    GUILayout.Height(64));
                                GUI.backgroundColor = oldcolor;

                                if (isBanned == newIsBanned) continue;

                                if (newIsBanned) _bannedEquipment.Add((int) equipmentDef.equipmentIndex);
                                else _bannedEquipment.Remove((int) equipmentDef.equipmentIndex);

                                pickupSetting.Value = SetToStringList(_bannedEquipment);
                                _config.Save();
                            }
                        }

                        GUILayout.EndHorizontal();
                        GUILayout.EndScrollView();
                        break;
                    }
                }
            }
        }

        private static string AddSpaces(string label)
        {
            return Regex.Replace(label, "([A-Z])", " $1");
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