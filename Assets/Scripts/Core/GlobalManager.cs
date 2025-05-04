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

        /// <summary>
        /// 启动模式：调试模式 = true, 正常模式 = false
        /// </summary>
        public bool IsDebugMode = false;

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
            if (IsDebugMode)
            {
                LaunchDebugMode();
            }
            else
            {
                LaunchNormalMode();
            }
        }

        private void InitializeFramework()
        {
            Debug.Log("<color=orange>[GlobalManager]</color> Initializing Framework...");

            FrameworkBootstrap.Initialize();

            Debug.Log("<color=orange>[GlobalManager]</color> Framework Initialized.");
        }

        // 新增方法：区分正常模式和调试模式启动流程
        private void LaunchNormalMode()
        {
            Debug.Log("<color=orange>[GlobalManager]</color> Starting Normal Mode...");
            // TODO: 从正规初始场景开始加载
            // TODO: 配置必要的游戏内容
            // TODO: 播放Logo等开屏动画
            InitializeFramework();
            Debug.Log("<color=orange>[GlobalManager]</color> Normal Mode Initialized.");
        }

        private void LaunchDebugMode()
        {
            Debug.Log("<color=orange>[GlobalManager]</color> Starting Debug Mode...");
            // TODO: 可选配置
            // TODO: 跳过Logo等开屏动画，直接进入游戏逻辑
            InitializeFramework();
            Debug.Log("<color=orange>[GlobalManager]</color> Debug Mode Initialized.");


            C_SceneManager.Instance.LoadSceneAsync<DemoScene>("DemoScene", () =>
            {
                Debug.Log("<color=orange>[GlobalManager]</color> Demo Scene Loaded.");
            });
        }

        private void OnApplicationQuit()
        {
            OnApplicationQuitEvent?.Invoke();
            Debug.Log("<color=orange>[GlobalManager]</color> Application quitting, shutting down framework.");
            FrameworkBootstrap.Shutdown();
        }
    }
}
