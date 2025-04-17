using System;
using System.Reflection;
using UnityEngine;

namespace CFramework.Core
{
    public abstract class BaseManager<T> where T : BaseManager<T>
    {
        private static readonly Lazy<T> instance = new Lazy<T>(() =>
        {
            try
            {
                return (T)typeof(T).InvokeMember(
                    name: null,
                    invokeAttr: BindingFlags.CreateInstance | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public,
                    binder: null,
                    target: null,
                    args: null
                );
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to create instance of {typeof(T).Name}: {ex.Message}");
                throw;
            }
        });

        public static T Instance => instance.Value;

        protected BaseManager()
        {
#if UNITY_EDITOR
            if (instance.IsValueCreated)
            {
                Debug.LogError($"Singleton instance of {typeof(T).Name} already exists.");
                return;
            }
#endif
        }

        public abstract void Initialize();

        public abstract void Shutdown();
    }
}
