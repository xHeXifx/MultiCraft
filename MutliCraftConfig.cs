using MultiCraft;
using MultiCraft.Helpers;
using Nautilus.Json;
using Nautilus.Options.Attributes;

[Menu($"MultiCraft (v{PluginInfo.PLUGIN_VERSION})")]
public class MultiCraftConfig : ConfigFile
{
    [Toggle("Check for updates")]
    public bool EnableUpdateChecker = true;
}