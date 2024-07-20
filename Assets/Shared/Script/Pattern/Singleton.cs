namespace Shared.Pattern
{
    using UnityEngine;

    public class Singleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var duplicates = FindObjectsByType(typeof(T), FindObjectsSortMode.None);

                    if (duplicates.Length > 0) // 场景中有脚本，但没有执行 Awake ，可能是因为脚本是后添加的
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
                // 这里只能用 as ，不能用 (T)，因为不存在从 Singleton(T) 到 T 的 Explicit Conversion
                // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/type-testing-and-cast#code-try-6
                // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/conversions#103-explicit-conversions
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