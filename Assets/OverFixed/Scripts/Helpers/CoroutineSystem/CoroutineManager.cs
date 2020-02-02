using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OverFixed.Scripts.Helpers.CoroutineSystem
{
    public class CoroutineManager : MonoBehaviour
    {
        private static CoroutineManager _instance;
        private static CoroutineManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject().AddComponent<CoroutineManager>();
                }

                return _instance;
            }
        }
                
        public static Coroutine StartChildCoroutine(IEnumerator method)
        {
            return Instance.StartCoroutine(method);
        }

        public static void StartChildCoroutine(string method)
        {
            Instance.StartCoroutine(method);
        }

        public static void StopChildCoroutine(Coroutine method)
        {
            if (method != null)
            {
                Instance.StopCoroutine(method);
            }

            method = null;
        }

        public static void StopChildCoroutine(string method)
        {
            Instance.StopCoroutine(method);
        }

        public static void DoAfterFixedUpdate(Action actionToInvoke)
        {
            Instance.StartCoroutine(Wait(Time.fixedDeltaTime, actionToInvoke));
        }

        public static Coroutine DoAfterGivenTime(float waitTime, Action actionToInvoke)
        {
            return Instance.StartCoroutine(Wait(waitTime, actionToInvoke));
        }

//        public static Coroutine DoAfterGivenTime(float waitTime, Action actionToInvoke)
//        {
//            return Instance.StartCoroutine(Wait(waitTime, actionToInvoke));
//        }

        public IEnumerator ProcessMultipleCoroutines(IEnumerable<IEnumerator> coroutineArray, Action actionToInvoke = null)
        {
            foreach (var enumerator in coroutineArray)
            {
                yield return StartCoroutine(enumerator);
            }

            actionToInvoke?.Invoke();
        }

        private static IEnumerator Wait(float time, Action actionToInvoke)
        {
            yield return new WaitForSecondsRealtime(time);

            actionToInvoke.Invoke();
        }
    }
}
