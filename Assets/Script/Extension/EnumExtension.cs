using System;
using System.Collections.Generic;
using UnityEngine;

namespace Yd.Extension
{
    public static class EnumExtension
    {
        private static Dictionary<Enum, string> enumStr;
        private static Dictionary<string, object> strEnum;
        private static Dictionary<Enum, string> EnumStr => enumStr ??= new();
        private static Dictionary<string, object> StrEnum => strEnum ??= new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            Debug.Log($"{nameof(EnumExtension)} reset.");

            EnumStr?.Clear();
            StrEnum?.Clear();
        }

        public static string GetString(this Enum e)
        {
            if (!EnumStr.TryGetValue(e, out var str))
            {
                str = Enum.GetName(e.GetType(), e);
                EnumStr.Add(e, str);
            }

            return str;

            // return Enum.GetName(e.GetType(), e);
        }

        public static T GetEnum<T>(this string s) where T : struct
        {
            if (!StrEnum.TryGetValue(s, out var e))
            {
                e = (Enum)Enum.Parse(typeof(T), s);
                StrEnum.Add(s, e);
            }

            return (T)e;

            // return Enum.Parse<T>(s);
        }
    }
}