using System.Collections;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MultiCraft.Helpers;
using Nautilus.Handlers;
using UnityEngine;

namespace MultiCraft;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    private static float lastTimeShowMessage;
    private static readonly float waitingForMessage = 1f;

    public new static ManualLogSource Logger { get; private set; }
    
    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();
    public static MultiCraftConfig MCConfig;
    public static Plugin Instance { get; private set; }

    private void Awake()
    {
        Logger = base.Logger;
        MCConfig = OptionsPanelHandler.RegisterModOptions<MultiCraftConfig>();

        LanguageHelper.Init();

        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        StartCoroutine(DelayedUpdateCheck());
    }

    public static bool ShowMessage(string str)
    {
        if (lastTimeShowMessage + waitingForMessage < Time.unscaledTime || lastTimeShowMessage > Time.unscaledTime)
        {
            ErrorMessage.AddWarning(str);
            lastTimeShowMessage = Time.unscaledTime;
            return true;
        }

        return false;
    }

    private IEnumerator DelayedUpdateCheck()
    {
        yield return new WaitForSeconds(5f);
        Logger.LogInfo("Checking for updates...");
        yield return UpdateHelper.checkForUpdate();
    }
    
}