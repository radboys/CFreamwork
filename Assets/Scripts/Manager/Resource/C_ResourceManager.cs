//#define USE_ADDRESSABLES

using CFramework.Core;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

#if USE_ADDRESSABLES
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
#endif

/// <summary>
/// 资源管理器，负责异步加载和管理游戏资源（支持 Resources 和 Addressables 两种方式）。
/// 通过 BaseManager<C_ResourceManager> 实现单例模式。
/// </summary>
public class C_ResourceManager : BaseManager<C_ResourceManager>
{
    // 私有构造，确保通过 BaseManager<Create>() 进行懒加载
    private C_ResourceManager() { }

    /// <summary>
    /// 初始化资源管理器。
    /// </summary>
    public override void Initialize()
    {
        Debug.Log("<color=green>[ResourceLoader]</color> Initializing...");
        Debug.Log("<color=green>[ResourceLoader]</color> Initialized.");
    }

    /// <summary>
    /// 关闭资源管理器，释放资源。
    /// </summary>
    public override void Shutdown()
    {
        Debug.Log("<color=green>[ResourceLoader]</color> Shutting down...");
        Debug.Log("<color=green>[ResourceLoader]</color> Shutdown.");
    }

    /// <summary>
    /// 异步加载 Resources 文件夹中的资源。
    /// </summary>
    /// <typeparam name="T">资源类型（如 GameObject, Sprite 等）。</typeparam>
    /// <param name="path">资源路径（相对于 Resources 文件夹）。</param>
    /// <param name="onComplete">加载完成后的回调函数。</param>
    public void LoadResourceAsync<T>(string path, UnityAction<T> onComplete) where T : Object
    {
        ResourceRequest request = Resources.LoadAsync<T>(path);
        request.completed += (operation) =>
        {
            if (request.asset == null)
            {
                Debug.LogError($"[ResourceLoader] Failed to load resource: {path}, due to unmatch type or asset not found.");
                onComplete?.Invoke(null);
                return;
            }

            onComplete?.Invoke(request.asset as T);
        };
    }

#if USE_ADDRESSABLES
    /// <summary>
    /// 异步加载 Addressables 系统中的资源。
    /// </summary>
    /// <typeparam name="T">资源类型（如 GameObject, Sprite 等）。</typeparam>
    /// <param name="key">Addressables 资源的键。</param>
    /// <param name="onComplete">加载完成后的回调函数。</param>
    public void LoadAddressableAsync<T>(string key, UnityAction<T> onComplete) where T : Object
    {
        var handle = Addressables.LoadAssetAsync<T>(key);
        handle.Completed += (operation) =>
        {
            if (operation.Status == AsyncOperationStatus.Succeeded)
            {
                onComplete?.Invoke(operation.Result as T);
            }
            else
            {
                Debug.LogError($"[ResourceLoader] Failed to load Addressable: {key}, due to unmatch type or asset not found.");
                onComplete?.Invoke(null);
            }
        };
    }
#endif

    /// <summary>
    /// 统一加载接口，根据 USE_ADDRESSABLES 宏自动选择 Resources 或 Addressables 方式加载资源。
    /// </summary>
    /// <typeparam name="T">资源类型（如 GameObject, Sprite 等）。</typeparam>
    /// <param name="keyOrPath">资源键或路径（Addressables 键或 Resources 路径）。</param>
    /// <param name="onComplete">加载完成后的回调函数。</param>
    public void LoadAsset<T>(string keyOrPath, UnityAction<T> onComplete) where T : Object
    {
#if USE_ADDRESSABLES
        LoadAddressableAsync<T>(keyOrPath, onComplete);
#else
        LoadResourceAsync<T>(keyOrPath, onComplete);
#endif
    }
}
