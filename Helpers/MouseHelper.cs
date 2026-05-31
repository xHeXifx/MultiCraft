using System;
using UnityEngine;

namespace MultiCraft.Helpers;

internal static class MouseHelper
{
    public enum ScrollDir
    {
        None,
        Up,
        Down,
    }

    public static string ScrollUpIcon => "ScrollUp";
    public static string ScrollDownIcon => "ScrollDown";

    public static ScrollDir GetScrollDir()
    {
        var scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        return scroll switch
        {
            0 => ScrollDir.None,
            < 0 => ScrollDir.Down,
            > 0 => ScrollDir.Up,
            _ => throw new NotImplementedException(),
        };
    }
}
