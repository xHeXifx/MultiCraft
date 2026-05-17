using HarmonyLib;
using MultiCraft.Handlers;

namespace MultiCraft.Patches;

[HarmonyPatch(typeof(Crafter))]
internal class CrafterPatch
{
    [HarmonyPrefix, HarmonyPatch("Craft")]
    public static void CraftPrefix(TechType techType, ref float duration)
    {
        if (CraftAmountHandler.main.IsHandlingTechType(techType))
            duration = CraftAmountHandler.main.GetActualCraftDuration(duration);
    }
}
