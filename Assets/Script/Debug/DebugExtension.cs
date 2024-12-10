using UnityEngine;
using Yd.Extension;

namespace Yd.DebugExtension
{
    public static class DebugE
    {
        public static void LogUnityObject(Object unityObject)
        {
            LogValue(unityObject.name, unityObject);
        }

        public static void LogValue<T>(string name, T value)
        {
            Debug.Log($"{name} = {value}");
        }

        public static void LogValue<T>(LogType logType, string name, T value)
        {
            Debug.unityLogger.Log(logType, $"{name} = {value}");
        }

        public static void DrawSphere(Vector3 origin, float radius, Color color = default, int segment = -1)
        {
            DrawShape(WireShape.Sphere(origin, radius, segment), color);
        }

        private static void DrawShape(Vector3[] shape, Color color)
        {
            for (var i = 0; i < shape.Length; i += 2)
            {
                Debug.DrawLine(shape[i], shape[i + 1], color);
            }
        }
    }
}