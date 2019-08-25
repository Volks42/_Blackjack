using System.Collections;
using UnityEngine;


public static class StaticCoroutine
{
    private class CoroutineHolder : MonoBehaviour { }

    private static CoroutineHolder _runner;
    private static CoroutineHolder Runner
    {
        get
        {
            if (_runner == null)
            {
                _runner = new GameObject("CoroutineHandler").AddComponent<CoroutineHolder>();
            }
            return _runner;
        }
    }

    public static void StartCoroutine(IEnumerator coroutine)
    {
        Runner.StartCoroutine(coroutine);
    }
}