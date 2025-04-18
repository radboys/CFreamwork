//#define USE_ADDRESSABLES

using CFramework.Core;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

#if USE_ADDRESSABLES
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
#endif

public class C_ResourceManager : BaseManager<C_ResourceManager>
{
    // 私有构造，确保通过 BaseManager<Create>() 进行懒加载
    private C_ResourceManager() { }

    public override void Initialize()
    {
        Debug.Log("[ResourceLoader] Initializing...");
        Debug.Log("[ResourceLoader] Initialized.");
    }

    public override void Shutdown()
    {
        Debug.Log("[ResourceLoader] Shutting down...");
        Debug.Log("[ResourceLoader] Shutdown.");
    }

    // 异步加载资源（Resources 文件夹）
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
    // 异步加载资源（Addressables）
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

    // 统一加载接口（自动选择 Resources 或 Addressables）
    public void LoadAsset<T>(string keyOrPath, UnityAction<T> onComplete) where T : Object
    {
#if USE_ADDRESSABLES
        LoadAddressableAsync<T>(keyOrPath, onComplete);
#else
        LoadResourceAsync<T>(keyOrPath, onComplete);
#endif
    }
}
