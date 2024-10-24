using Unity.VisualScripting;
using UnityEngine;

namespace Yd.Extension
{
    public static class MonoBehaviourE
    {
        public static T GetOrAddComponent<T>(this MonoBehaviour behaviour) where T : Component
        {
            return behaviour.gameObject.GetOrAddComponent<T>();
        }
    }
}