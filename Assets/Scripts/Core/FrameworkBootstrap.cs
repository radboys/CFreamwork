using System;
using UnityEngine;
using CFramework.Managers;
namespace CFramework.Core
{
    /// <summary>
    /// 框架引导程序，负责管理所有核心系统的生命周期
    /// </summary>
    public static class FrameworkBootstrap
    {
        private static bool isInitialized = false;

        /// <summary>
        /// 初始化框架
        /// </summary>
        public static void Initialize()
        {
            if (isInitialized)
            {
                return;
            }

            isInitialized = true;

            InitializeManager();
        }

        /// <summary>
        /// 初始化所有管理器
        /// 按照依赖关系顺序进行初始化，确保系统正确启动
        /// </summary>
        private static void InitializeManager()
        {
            Debug.Log("<color=orange>[FrameworkBootstrap]</color> Initializing managers...");

            // 创建全局更新调度器
            new GameObject("UpdateDispatcher").AddComponent<UpdateDispatcher>();

            // ===== 框架层 =====
            // 1. 配置管理：加载全局配置
            // ConfigManager.Instance.Load(configPath);

            // 2. 事件系统：模块间通信的基础设施
            C_EventManager.Instance.Initialize();

            // ===== 系统层 =====
            // 3. 资源管理：负责资源的加载、卸载和缓存
            C_ResourceManager.Instance.Initialize();

            // 4. 输入系统：处理用户输入，为其他系统提供输入服务
            C_InputManager.Instance.Initialize();

            // 5. 对象池：提供对象复用功能，优化性能
            C_ObjectPoolManager.Instance.Initialize();

            // 6. 场景管理：处理场景切换和加载
            C_SceneManager.Instance.Initialize();

            // ===== 游戏层 =====
            // 7. 游戏状态：管理游戏流程和状态转换
            C_GameStateManager.Instance.Initialize();

            // 8. UI管理：处理界面显示和交互
            C_UIManager.Instance.Initialize();

            // 9. 音频管理：处理游戏音效和背景音乐
            C_AudioManager.Instance.Initialize();

            Debug.Log("<color=orange>[FrameworkBootstrap]</color> Managers initialized.");
        }

        /// <summary>
        /// 关闭框架
        /// </summary>
        public static void Shutdown()
        {
            if (!isInitialized)
            {
                return;
            }

            isInitialized = false;

            ShutdownManager();
        }

        /// <summary>
        /// 关闭所有管理器
        /// 按照与初始化相反的顺序进行关闭，确保依赖安全
        /// </summary>
        private static void ShutdownManager()
        {
            Debug.Log("<color=orange>[FrameworkBootstrap]</color> Shutting down managers...");

            // ===== 游戏层 =====
            // 9. 音频管理
            C_AudioManager.Instance.Shutdown();

            // 8. UI管理
            C_UIManager.Instance.Shutdown();

            // 7. 游戏状态
            C_GameStateManager.Instance.Shutdown();

            // ===== 系统层 =====
            // 6. 场景管理
            C_SceneManager.Instance.Shutdown();

            // 5. 对象池
            C_ObjectPoolManager.Instance.Shutdown();

            // 4. 输入系统
            C_InputManager.Instance.Shutdown();

            // 3. 资源管理
            C_ResourceManager.Instance.Shutdown();

            // ===== 框架层 =====
            // 2. 事件系统
            C_EventManager.Instance.Shutdown();

            // 删除更新调度器
            GameObject.Destroy(UpdateDispatcher.Instance.gameObject);

            Debug.Log("<color=orange>[FrameworkBootstrap]</color> Managers shutdown.");
        }
    }
}
