using System;
using System.Collections;
using UnityEngine;
using Yd.Pattern;
using Random = UnityEngine.Random;

namespace Yd.Manager
{
    public class CoroutineTimer : MonoSingleton<CoroutineTimer>
    {
        public static Coroutine SetTimer(Action<CoroutineTimerCallbackContext> callback, float time)
        {
            return Instance.StartCoroutine(Coroutine(callback, () => time, true, CoroutineTimerLoopPolicy.Once));
        }

        public static Coroutine SetTimer(
            Action<CoroutineTimerCallbackContext> callback, float time, CoroutineTimerLoopPolicy loopPolicy
        )
        {
            return Instance.StartCoroutine(Coroutine(callback, () => time, true, loopPolicy));
        }

        public static Coroutine SetTimer(
            Action<CoroutineTimerCallbackContext> actionCallback, Func<float> timeCallback, CoroutineTimerLoopPolicy loopPolicy
        )
        {
            return Instance.StartCoroutine(Coroutine(actionCallback, timeCallback, false, loopPolicy));
        }

        public static Coroutine SetTimer(
            Action<CoroutineTimerCallbackContext> actionCallback, float minTime, float maxTime,
            CoroutineTimerLoopPolicy loopPolicy
        )
        {
            if (maxTime == 0 && loopPolicy.isInfiniteLoop)
            {
                Debug.LogError("Infinite loop must has a duration > 0.");
                return null;
            }

            return Instance.StartCoroutine(Coroutine(actionCallback, () => Random.Range(minTime, maxTime), false, loopPolicy));
        }

        public static void Cancel(ref Coroutine coroutine)
        {
            if (coroutine != null)
            {
                Instance.StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        public static void CancelAll()
        {
            Instance.StopAllCoroutines();
        }

        private static IEnumerator Coroutine(
            Action<CoroutineTimerCallbackContext> callback, Func<float> timeCallback, bool constTime,
            CoroutineTimerLoopPolicy loopPolicy
        )
        {
            if (loopPolicy.invokeImmediately && loopPolicy.TryLoop())
            {
                callback?.Invoke(new CoroutineTimerCallbackContext(loopPolicy));
            }

            float time;
            WaitForSeconds waitForSeconds = null;

            if (constTime)
            {
                time = timeCallback();
                if (time != 0)
                {
                    waitForSeconds = new WaitForSeconds(time);
                }
            }

            while (loopPolicy.TryLoop())
            {
                if (!constTime)
                {
                    time = timeCallback();
                    if (time != 0)
                    {
                        waitForSeconds = new WaitForSeconds(time);
                    }
                }

                if (waitForSeconds != null)
                {
                    yield return waitForSeconds;
                }

                callback?.Invoke(new CoroutineTimerCallbackContext(loopPolicy));
            }
        }
    }

    [Serializable]
    public struct CoroutineTimerLoopPolicy
    {
        public static readonly CoroutineTimerLoopPolicy Once = new()
            { isInfiniteLoop = false, invokeImmediately = false, loopCount = 1 };
        public static readonly CoroutineTimerLoopPolicy InfiniteLoop = new()
            { isInfiniteLoop = true, invokeImmediately = false, loopCount = 0 };
        public bool isInfiniteLoop;
        public bool invokeImmediately;
        public int loopCount;

        public int RemainingLoopCount => loopCount;

        public bool TryLoop()
        {
            return isInfiniteLoop || loopCount-- > 0;
        }
    }

    public struct CoroutineTimerCallbackContext
    {
        public readonly CoroutineTimerLoopPolicy LoopPolicy;

        public CoroutineTimerCallbackContext(CoroutineTimerLoopPolicy loopPolicy)
        {
            LoopPolicy = loopPolicy;
        }
    }
}