using System.Reflection;
using HarmonyLib;

namespace MultiCraft.Helpers;

internal static class TooltipVisibility
{
    private static readonly FieldInfo VisibleField = AccessTools.Field(typeof(uGUI_Tooltip), "visible");

    public static bool IsVisible()
    {
        if (VisibleField == null)
            return true;

        return (bool)VisibleField.GetValue(null);
    }
}
