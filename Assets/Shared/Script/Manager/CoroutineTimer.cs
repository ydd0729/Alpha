using System.Collections;
using System;
using Shared.Pattern;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Shared.Manager
{
    public class CoroutineTimer : MonoSingleton<CoroutineTimer>
    {
        public static Coroutine SetTimer(UnityAction callback,float time)
        {
            return Instance.StartCoroutine(Coroutine(callback, () => time,true, LoopPolicy.Once));
        }
    
        public static Coroutine SetTimer(UnityAction callback,float time,  LoopPolicy loopPolicy)
        {
            return Instance.StartCoroutine(Coroutine(callback, () => time,true, loopPolicy));
        }
        
        public static Coroutine SetTimer(UnityAction actionCallback, Func<float> timeCallback, LoopPolicy loopPolicy)
        {
            return Instance.StartCoroutine(Coroutine(actionCallback, timeCallback,false, loopPolicy));
        }
        
        public static Coroutine SetTimer(UnityAction actionCallback, float minTime, float maxTime, LoopPolicy loopPolicy)
        {
            if (maxTime == 0 && loopPolicy.IsInfiniteLoop)
            {
                Debug.LogError("Infinite loop must has a duration > 0.");
                return null;
            }
            
            return Instance.StartCoroutine(Coroutine(actionCallback, () => Random.Range(minTime, maxTime),false, loopPolicy));
        }
        
        public static void Cancel(ref Coroutine coroutine)
        {
            if (coroutine != null)
            {
                Instance.StopCoroutine(coroutine);
                coroutine = null;
            }
        }
        
        private static IEnumerator Coroutine(UnityAction callback, Func<float> timeCallback,bool constTime, LoopPolicy loopPolicy)
        {
            if (loopPolicy.InvokeImmediately && loopPolicy.TryLoop())
            {
                callback?.Invoke();
            }
            
            WaitForSeconds waitForSeconds = null;
            
            if (constTime)
            {
                waitForSeconds = new WaitForSeconds(timeCallback());
            }
            
            while (loopPolicy.TryLoop())
            {
                if (!constTime)
                {
                    waitForSeconds = new WaitForSeconds(timeCallback());
                }
                yield return waitForSeconds;
                callback?.Invoke();
            }
        }
    }
    
    [Serializable]
    public struct LoopPolicy
    {
        public bool IsInfiniteLoop;
        public bool InvokeImmediately;
        public int LoopCount;
    
        public bool TryLoop()
        {
            return IsInfiniteLoop || LoopCount-- > 0;
        }
    
        public static readonly LoopPolicy Once = new() { IsInfiniteLoop = false, InvokeImmediately = false, LoopCount = 1 };
        public static readonly LoopPolicy InfiniteLoop = new() { IsInfiniteLoop = true, InvokeImmediately = false, LoopCount = 0 };
    }
}