using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareSuite
{
    public static class ModCompatibilityInLobbyConfig
    {
        private static bool? _enabled;

        public static bool enabled
        {
            get
            {
                if (_enabled == null)
                {
                    _enabled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.KingEnderBrine.InLobbyConfig");
                }
                return (bool)_enabled;
            }
        }
        public static void CreateFromBepInExConfigFile(ConfigFile config, string displayName)
        {
            var configEntry = InLobbyConfig.Fields.ConfigFieldUtilities.CreateFromBepInExConfigFile(config, displayName);
            InLobbyConfig.ModConfigCatalog.Add(configEntry);
        }
    }
}
