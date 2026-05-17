using Nautilus.Handlers;

namespace MultiCraft.Helpers;

internal static class LanguageHelper
{
    public static void Init()
    {
        LanguageHandler.SetLanguageLine("NotEnoughPower", "Not enough power to craft the selected amount!", "English");
        LanguageHandler.SetLanguageLine("ChangeItemAmount", "change quantity", "English");

        LanguageHandler.SetLanguageLine("NotEnoughPower", "No hay suficiente energia para tanta cantidad!", "Spanish");
        LanguageHandler.SetLanguageLine("ChangeItemAmount", "cambiar cantidad", "Spanish");

        LanguageHandler.SetLanguageLine("NotEnoughPower", "No hay suficiente energia para tanta cantidad!", "Spanish (Latin America)");
        LanguageHandler.SetLanguageLine("ChangeItemAmount", "cambiar cantidad", "Spanish (Latin America)");
    }
}
