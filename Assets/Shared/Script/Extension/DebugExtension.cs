using UnityEngine;

namespace Shared.Extension
{
    public static class DebugExtension
    {
        public static void LogUnityObject(Object unityObject)
        {
            LogValue(unityObject.name, unityObject);
        }
        
        public static void LogValue<T>(string name, T value)
        {
            Debug.Log($"{name} = {value}");
        }
    }
}