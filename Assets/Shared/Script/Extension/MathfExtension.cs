using UnityEngine;

namespace Shared.Extension
{
    public static class MathfExtension
    {
        public static float AutoClamp(float v, float a, float b)
        {
            float min = Mathf.Min(a, b);
            float max = Mathf.Max(a, b);
            return Mathf.Clamp(v, min, max);
        }
    }
}