using HarmonyLib;
using MultiCraft.Handlers;

namespace MultiCraft.Patches;

[HarmonyPatch(typeof(TooltipFactory))]
internal class TooltipFactoryPatch
{
    [HarmonyPostfix, HarmonyPatch(nameof(TooltipFactory.CraftRecipe))]
    public static void CraftRecipePostfix(TechType techType, bool locked, TooltipData data)
    {
        if (locked)
            return;

        CraftAmountHandler.main.UpdateCraftAmount(techType);
        ToolTipHandler.UpdateActionHint(techType, data.postfix);
    }
}
