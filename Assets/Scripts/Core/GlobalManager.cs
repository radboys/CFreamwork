using System;
using UnityEngine;

namespace CFramework.Core
{
    /// <summary>
    /// 全局流程管理器：负责框架启动／关闭、配置加载、全局生命周期事件广播。
    /// </summary>
    public class GlobalManager : MonoBehaviour
    {
        /// <summary>
        /// 全局流程管理器实例
        /// </summary>
        public static GlobalManager Instance { get; private set; }

        /// <summary>应用即将退出时触发</summary>
        public static event Action OnApplicationQuitEvent;

        private void Awake()
        {
            // —— 单例保护 ——
            var exists = FindObjectsByType<GlobalManager>(FindObjectsSortMode.None);
            if (exists.Length > 1)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // —— 启动流程 ——
            InitializeFramework();
        }

        private void InitializeFramework()
        {
            Debug.Log("[GameEntry] Initializing Framework...");
            FrameworkBootstrap.Initialize();
            Debug.Log("[GameEntry] Framework Initialized.");
        }

        private void OnApplicationQuit()
        {
            OnApplicationQuitEvent?.Invoke();
            Debug.Log("[GameEntry] Application quitting, shutting down framework.");
            FrameworkBootstrap.Shutdown();
        }
    }
}
