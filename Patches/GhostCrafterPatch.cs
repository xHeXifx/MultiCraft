using System.Reflection;
using HarmonyLib;
using MultiCraft.Handlers;

namespace MultiCraft.Patches;

[HarmonyPatch(typeof(GhostCrafter))]
internal class GhostCrafterPatch
{
    private static readonly FieldInfo PowerRelayField = AccessTools.Field(typeof(GhostCrafter), "powerRelay");

    private static PowerRelay GetPowerRelay(GhostCrafter instance)
    {
        return PowerRelayField?.GetValue(instance) as PowerRelay;
    }

    [HarmonyPrefix, HarmonyPatch("Craft")]
    public static bool CraftPrefix(GhostCrafter __instance, TechType techType, ref bool __state)
    {
        __state = __instance.needsPower;

        if (!CraftAmountHandler.main.IsHandlingTechType(techType))
            return true;

        if (!__instance.needsPower)
            return true;

        var powerRelay = GetPowerRelay(__instance);
        if (!CraftAmountHandler.main.CanCraftAmount(powerRelay))
        {
            MultiCraft.Plugin.ShowMessage(Language.main.Get("NotEnoughPower"));
            __instance.CancelInvoke("Craft");
            return false;
        }

        CrafterLogic.ConsumeEnergy(powerRelay, CraftAmountHandler.main.GetActualEnergyCost());
        __instance.needsPower = false;

        return true;
    }

    [HarmonyPostfix, HarmonyPatch("Craft")]
    public static void CraftPostfix(GhostCrafter __instance, bool __state)
    {
        __instance.needsPower = __state;
    }
}
