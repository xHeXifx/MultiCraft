using MultiCraft.Helpers;
using System.Text;

namespace MultiCraft.Handlers;

internal static class ToolTipHandler
{
    private enum ActionHint
    {
        None = 0,
        Increase = 1,
        Decrease = 2,
        Both = 3
    }

    public static void UpdateActionHint(TechType techType, StringBuilder tooltip)
    {
        if (!CraftAmountHandler.main.IsHandlingTechType(techType))
            return;

        var hintType = GetActionHint();
        var text = GetActionText(hintType);

        tooltip.AppendLine(text);
    }

    private static ActionHint GetActionHint()
    {
        var craftAmount = CraftAmountHandler.main.CraftAmount;
        var maxCraftAmount = CraftAmountHandler.main.MaxCraftAmount;

        if (maxCraftAmount <= 1)
            return ActionHint.None;
        if (craftAmount == 1)
            return ActionHint.Increase;
        if (craftAmount == maxCraftAmount)
            return ActionHint.Decrease;

        return ActionHint.Both;
    }

    private static string GetActionText(ActionHint hintType)
    {
        return hintType switch
        {
            ActionHint.Increase => GetToolTip(MouseHelper.ScrollUpIcon),
            ActionHint.Decrease => GetToolTip(MouseHelper.ScrollDownIcon),
            ActionHint.Both => GetToolTip($"{MouseHelper.ScrollUpIcon}/{MouseHelper.ScrollDownIcon}"),
            _ => string.Empty
        };
    }

    private static string GetToolTip(string hint)
        => "\n<size=20>" + hint + " - <color=#00ffffff>" + Language.main.Get("ChangeItemAmount") + "</color></size>";
}
