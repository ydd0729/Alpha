using System;
using UnityEngine;

namespace Yd.Extension
{
    public static class StaticColor
    {
        public static Color Get(DebugColorType type)
        {
            switch(type)
            {

                case DebugColorType.Red:
                    return Color.red;
                case DebugColorType.Green:
                    return Color.green;
                case DebugColorType.Blue:
                    return Color.blue;
                case DebugColorType.White:
                    return Color.white;
                case DebugColorType.Black:
                    return Color.black;
                case DebugColorType.Yellow:
                    return Color.yellow;
                case DebugColorType.Cyan:
                    return Color.cyan;
                case DebugColorType.Magenta:
                    return Color.magenta;
                case DebugColorType.Grey:
                    return Color.grey;
                case DebugColorType.clear:
                    return Color.clear;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }

    public enum DebugColorType
    {
        Red,
        Green,
        Blue,
        White,
        Black,
        Yellow,
        Cyan,
        Magenta,
        Grey,
        clear
    }
}