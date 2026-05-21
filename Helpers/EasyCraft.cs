using MultiCraft.Shared;
using System;
using System.Reflection;

namespace MultiCraft.Helpers;

internal class EasyCraft
{
    private const BindingFlags AllFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
    private static readonly MethodWrapper<Func<TechType, int>> getPickupCount;

    static EasyCraft()
    {
        var getPickupCountMethod = Type
          .GetType("EasyCraft.ClosestItemContainers, EasyCraft_BZ")
          ?.GetMethod("GetPickupCount", AllFlags);

        getPickupCount = getPickupCountMethod != null ? new MethodWrapper<Func<TechType, int>>(getPickupCountMethod) : null;
    }

    public static int? GetPickupCount(TechType techType) => getPickupCount?.Invoke(techType);
}
