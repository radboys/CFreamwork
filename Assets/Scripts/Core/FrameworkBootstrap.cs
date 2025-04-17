using System;
using UnityEngine;
using CFramework.Managers;
namespace CFramework.Core
{
    public static class FrameworkBootstrap
    {
        private static bool isInitialized = false;

        public static void Initialize()
        {
            if (isInitialized)
            {
                return;
            }

            isInitialized = true;

            InitializeManager();
        }

        private static void InitializeManager()
        {
            Debug.Log("[FrameworkBootstrap] Initializing managers...");

            // —— 框架层 ——
            // 1. 配置管理：先加载全局配置
            //ConfigManager.Instance.Load(configPath);

            // 2. 日志/调试：根据配置打开调试面板或日志系统
            //if (FlowManager.Instance.EnableDebugPanel)
            //    DebugPanel.Initialize();
            //LogManager.Instance.Initialize();

            // 3. 事件总线：模块通信基础
            EventCenter.Instance.Initialize();

            // —— 系统层 ——
            // 4. 定时器：冷却／延迟调用等
            //TimerManager.Instance.Initialize();

            // 5. 输入系统：先于其他依赖输入的模块
            //InputManager.Instance.Initialize();

            // 6. 对象池：许多模块可能会从池里取物体
            //ObjectPoolManager.Instance.Initialize();

            // 7. 场景加载：准备好切场景逻辑
            //SceneLoader.Instance.Initialize();

            // —— 游戏层 ——
            // 8. 游戏状态机：在框架与系统都就绪后才启动
            GameStateManager.Instance.Initialize();

            // 9. UI 逻辑：依赖状态机与资源
            //UILogicManager.Instance.Initialize();

            // 10. 音频管理：可以在 UI、状态机后统一播放 BGM
            //AudioManager.Instance.Initialize();

            // 11. 存档管理：最后注册，保证所有数据模块都已准备好
            //SaveLoadManager.Instance.Initialize();

            Debug.Log("[FrameworkBootstrap] Managers initialized.");
        }

        public static void Shutdown()
        {
            if (!isInitialized)
            {
                return;
            }

            isInitialized = false;

            ShutdownManager();
        }

        private static void ShutdownManager()
        {
            Debug.Log("[FrameworkBootstrap] Shutting down managers...");

            // 反向顺序，保障依赖安全
            //SaveLoadManager.Instance.Shutdown();
            //AudioManager.Instance.Shutdown();
            //UILogicManager.Instance.Shutdown();
            //GameStateManager.Instance.Shutdown();
            //SceneLoader.Instance.Shutdown();
            //ObjectPoolManager.Instance.Shutdown();
            //InputManager.Instance.Shutdown();
            //TimerManager.Instance.Shutdown();
            //EventBus.Instance.Shutdown();
            //LogManager.Instance.Shutdown();
            //DebugPanel.Shutdown();        // 如果有
            //ConfigManager.Instance.Shutdown();

            Debug.Log("[FrameworkBootstrap] Managers shutdown.");
        }
    }
}
