using HarmonyLib;
using MultiCraft.Handlers;

namespace MultiCraft.Patches;

[HarmonyPatch(typeof(uGUI_CraftingMenu))]
internal class CraftingMenuPatch
{
    [HarmonyPostfix, HarmonyPatch(nameof(uGUI_CraftingMenu.Open))]
    public static void OpenPostfix(CraftTree.Type treeType, ITreeActionReceiver receiver)
        => CraftAmountHandler.main.SetCurrentTreeType(treeType);
}
