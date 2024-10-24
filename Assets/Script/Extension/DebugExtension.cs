using UnityEngine;

namespace Yd.Extension
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

        public static void DrawSphere(Vector3 origin, float radius, Color color = default, int segment = 16)
        {
            var unitAngleY = 360f / segment;
            var unitAngleZ = 180f / segment;

            var unitRotationZ = Quaternion.AngleAxis(-unitAngleY, Vector3.up); // 绕 z 轴反向转 1 下

            for (var i = 1; i <= segment; i++)
            {
                var rotationY = Quaternion.AngleAxis(unitAngleZ * i, Vector3.right); // 绕 y 轴转 i 下

                for (var j = 1; j <= segment; j++)
                {
                    var rotationZ = Quaternion.AngleAxis(unitAngleY * j, Vector3.up); // 绕 z 轴转 j 下
                    var start = rotationZ * rotationY * Vector3.up * radius;

                    var end = unitRotationZ * start; // 左边那个点
                    Debug.DrawLine(origin + start, origin + end, color);

                    end = Quaternion.AngleAxis(-unitAngleZ, Vector3.Cross(Vector3.up, start)) * start; // 上边那个点
                    Debug.DrawLine(origin + start, origin + end, color);
                }
            }
        }
    }
}