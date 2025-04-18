using UnityEngine;

namespace CFramework.Tools
{
    public static class GameObjectTools
    {
        /// <summary>
        /// Delays the execution of an action by a specified time.
        /// </summary>
        /// <param name="monoBehaviour">The MonoBehaviour to attach the coroutine to.</param>
        /// <param name="delay">The delay in seconds.</param>
        /// <param name="action">The action to execute after the delay.</param>
        public static void DelayAction(MonoBehaviour monoBehaviour, float delay, System.Action action)
        {
            monoBehaviour.StartCoroutine(DelayCoroutine(delay, action));
        }

        private static System.Collections.IEnumerator DelayCoroutine(float delay, System.Action action)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
    }
}
