using UnityEngine;

namespace Shared.Pattern
{
    public class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var duplicates = FindObjectsByType(typeof(T), FindObjectsSortMode.None);

                    if (duplicates.Length > 0) // 场景中有脚本，但没有执行 Awake ，比如当脚本是后添加的时
                    {
                        _instance = (T)duplicates[0];
                        DontDestroyOnLoad(_instance.gameObject);

                        for (int i = 1; i < duplicates.Length; i++)
                        {
                            Destroy(((T)duplicates[i]).gameObject);
                        }
                    }
                    else // 如果没有在场景中挂载，就会在使用时才创建，实现 Lazy Instantiation
                    {
                        GameObject gameObj = new(name: typeof(T).Name);
                        _instance = gameObj.AddComponent<T>();
                        DontDestroyOnLoad(gameObj);
                    }
                }

                return _instance;
            }
        }


        public virtual void Awake() // 如果在场景中挂载了脚本，会执行 Awake
        {
            if (_instance == null)
            {
                _instance = this as T;
                // _instance = GetComponent<T>();
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private static T _instance;
    }
}