using MultiCraft.Helpers;
using MultiCraft.Shared;
using Nautilus.Crafting;
using Nautilus.Handlers;
using System.Linq;

namespace MultiCraft.Handlers;

using ScrollDir = MouseHelper.ScrollDir;

internal sealed class CraftAmountHandler
{
    private CraftAmountHandler() { }

    private static readonly CraftAmountHandler instance = new();
    private CraftTree.Type currentTreeType = CraftTree.Type.None;
    private TechType currentTechType = TechType.None;
    private RecipeData originalRecipe;
    private RecipeData currentRecipe;
    private int craftAmount;
    private int maxCraftAmount;

    public static CraftAmountHandler main => instance;
    public int CraftAmount => craftAmount;
    public int MaxCraftAmount => maxCraftAmount;

    public bool IsHandlingTechType(TechType techType) => currentTechType == techType;
    public float GetActualCraftDuration(float baseDuration = 0.5f) => baseDuration * craftAmount;
    public float GetActualEnergyCost() => GetCurrentEnergyCost() * craftAmount;
    public void SetCurrentTreeType(CraftTree.Type treeType) => currentTreeType = treeType;

    public void UpdateCraftAmount(TechType techType)
    {
        if (!CanHandle())
            return;

        if (techType != currentTechType && currentTechType != TechType.None)
            Reset();

        if (currentTechType == TechType.None)
            Manage(techType);

        UpdateAmount();
    }

    public void ResetIfToolTipHidden()
    {
        if (!TooltipVisibility.IsVisible() && currentTechType != TechType.None)
            Reset();
    }

    public void Reset()
    {
        if (originalRecipe != null && currentTechType != TechType.None)
            CraftDataHandler.SetRecipeData(currentTechType, originalRecipe);

        currentTechType = TechType.None;
        originalRecipe = currentRecipe = null;
        craftAmount = maxCraftAmount = 0;
    }

    public bool CanCraftAmount(PowerRelay powerRelay)
    {
        if (!GameModeManager.GetOption<bool>(GameOption.TechnologyRequiresPower))
            return true;
        if (powerRelay == null)
            return false;

        var energy = GetActualEnergyCost();
        var power = powerRelay.GetPower();

        return power >= energy;
    }

    private void Manage(TechType techType)
    {
        RecipeData recipeData = CraftDataHandler.GetRecipeData(techType);
        if (recipeData == null)
            return;

        maxCraftAmount = recipeData.GetMaxCraftAmount();
        if (maxCraftAmount == 0)
            return;

        currentTechType = techType;
        originalRecipe = recipeData;
        craftAmount = 1;

        currentRecipe = new()
        {
            craftAmount = originalRecipe.craftAmount,
            Ingredients = new(originalRecipe.Ingredients),
            LinkedItems = new(originalRecipe.LinkedItems)
        };
    }

    private void UpdateAmount()
    {
        var dir = MouseHelper.GetScrollDir();
        if (!CanUpdateAmount(dir))
            return;

        craftAmount += dir == ScrollDir.Up ? 1 : -1;
        var newIngredients = originalRecipe.Ingredients
          .Select(ing => new Ingredient(ing.techType, ing.amount * craftAmount))
          .ToList();

        currentRecipe.craftAmount = originalRecipe.craftAmount * craftAmount;
        currentRecipe.Ingredients = newIngredients;

        CraftDataHandler.SetRecipeData(currentTechType, currentRecipe);
    }

    private bool CanHandle() => currentTreeType != CraftTree.Type.Constructor;

    private bool CanUpdateAmount(ScrollDir dir)
    {
        if (dir == ScrollDir.None || craftAmount == 0)
            return false;

        if (dir == ScrollDir.Up && craftAmount == maxCraftAmount)
            return false;

        if (dir == ScrollDir.Down && craftAmount == 1)
            return false;

        return true;
    }

    private float GetCurrentEnergyCost()
    {
        var exists = TechData.GetEnergyCost(currentTechType, out var retVal);
        return exists ? retVal : 1f;
    }
}
