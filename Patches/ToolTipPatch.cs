using HarmonyLib;
using MultiCraft.Handlers;

namespace MultiCraft.Patches;

[HarmonyPatch(typeof(uGUI_Tooltip))]
internal class ToolTipPatch
{
    [HarmonyPostfix, HarmonyPatch("OnUpdate")]
    public static void OnUpdatePostfix() => CraftAmountHandler.main.ResetIfToolTipHidden();
}
