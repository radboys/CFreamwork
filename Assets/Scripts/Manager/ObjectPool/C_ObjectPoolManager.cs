using System;
using System.Collections.Generic;
using UnityEngine;
using CFramework.Core;
using CFramework.Managers;

namespace CFramework.Managers
{
    /// <summary>
    /// 对象池管理器：管理游戏对象的复用
    /// </summary>
    public class C_ObjectPoolManager : BaseManager<C_ObjectPoolManager>
    {
        private Dictionary<string, ObjectPool> poolDictionary = new Dictionary<string, ObjectPool>();
        private const int DEFAULT_POOL_SIZE = 100;
        private const int MAX_POOL_SIZE = 1000;
        private GameObject poolRoot;

        // 私有构造，确保通过 BaseManager<Create>() 进行懒加载
        private C_ObjectPoolManager() { }

        public override void Initialize()
        {
            Debug.Log("<color=green>[C_ObjectPoolManager]</color> Initializing...");
            poolDictionary.Clear();
            CreatePoolRoot();
            Debug.Log("<color=green>[C_ObjectPoolManager]</color> Initialized.");
        }

        public override void Shutdown()
        {
            Debug.Log("<color=green>[C_ObjectPoolManager]</color> Shutting down...");
            ClearAllPools();
            if (poolRoot != null)
            {
                UnityEngine.Object.Destroy(poolRoot);
                poolRoot = null;
            }
            Debug.Log("<color=green>[C_ObjectPoolManager]</color> Shutdown.");
        }

        private void CreatePoolRoot()
        {
            if (poolRoot == null)
            {
                poolRoot = new GameObject("ObjectPools");
                poolRoot.transform.position = Vector3.zero;
                poolRoot.transform.rotation = Quaternion.identity;
                poolRoot.transform.localScale = Vector3.one;
                UnityEngine.Object.DontDestroyOnLoad(poolRoot);
            }
        }

        /// <summary>
        /// 创建对象池
        /// </summary>
        /// <param name="poolName">池名称</param>
        /// <param name="prefab">预制体</param>
        /// <param name="maxSize">最大大小</param>
        public void CreatePool(string poolName, GameObject prefab, int maxSize = DEFAULT_POOL_SIZE)
        {
            if (string.IsNullOrEmpty(poolName))
            {
                Debug.LogError("<color=green>[C_ObjectPoolManager]</color> Pool name cannot be null or empty.");
                return;
            }

            if (prefab == null)
            {
                Debug.LogError("<color=green>[C_ObjectPoolManager]</color> Prefab cannot be null.");
                return;
            }

            if (maxSize <= 0 || maxSize > MAX_POOL_SIZE)
            {
                Debug.LogWarning($"<color=green>[C_ObjectPoolManager]</color> Invalid max size {maxSize}, using default value {DEFAULT_POOL_SIZE}");
                maxSize = DEFAULT_POOL_SIZE;
            }

            if (poolDictionary.ContainsKey(poolName))
            {
                Debug.LogWarning($"<color=green>[C_ObjectPoolManager]</color> Pool {poolName} already exists.");
                return;
            }

            CreatePoolRoot();
            var pool = new ObjectPool(prefab, maxSize, poolRoot.transform, poolName);
            poolDictionary.Add(poolName, pool);
        }

        /// <summary>
        /// 从对象池获取对象
        /// </summary>
        /// <param name="prefabName">预制体名称</param>
        /// <param name="callback">获取对象的回调</param>
        public void GetObject(string prefabName, Action<GameObject> callback)
        {
            if (string.IsNullOrEmpty(prefabName))
            {
                Debug.LogError("<color=green>[C_ObjectPoolManager]</color> Prefab name cannot be null or empty.");
                callback?.Invoke(null);
                return;
            }

            if (poolDictionary.TryGetValue(prefabName, out var pool))
            {
                var obj = pool.GetObject();
                obj.SetActive(true);
                callback?.Invoke(obj);
                return;
            }

            // 通过ResourceLoader加载预制体
            C_ResourceManager.Instance.LoadResourceAsync<GameObject>(prefabName, (prefab) =>
            {
                if (prefab == null)
                {
                    Debug.LogError($"<color=green>[C_ObjectPoolManager]</color> Failed to load prefab: {prefabName}");
                    callback?.Invoke(null);
                    return;
                }

                CreatePool(prefabName, prefab);
                pool = poolDictionary[prefabName];
                var obj = pool.GetObject();
                obj.SetActive(true);
                callback?.Invoke(obj);
            });
        }

        /// <summary>
        /// 将对象返回池中
        /// </summary>
        /// <param name="poolName">池名称</param>
        /// <param name="obj">要返回的游戏对象</param>
        public void PushObject(string poolName, GameObject obj)
        {
            if (string.IsNullOrEmpty(poolName))
            {
                Debug.LogError("<color=green>[C_ObjectPoolManager]</color> Pool name cannot be null or empty.");
                return;
            }

            if (obj == null)
            {
                Debug.LogError("<color=green>[C_ObjectPoolManager]</color> Object cannot be null.");
                return;
            }

            if (poolDictionary.TryGetValue(poolName, out var pool))
            {
                pool.PushObject(obj);
                return;
            }

            C_ResourceManager.Instance.LoadResourceAsync<GameObject>(obj.name, (prefab) =>
            {
                if (prefab == null)
                {
                    Debug.LogError($"<color=green>[C_ObjectPoolManager]</color> Failed to load prefab: {obj.name}");
                    return;
                }

                CreatePool(obj.name, prefab);
                pool = poolDictionary[obj.name];
                pool.PushObject(obj);
            });
        }

        /// <summary>
        /// 清理所有对象池
        /// </summary>
        public void ClearAllPools()
        {
            foreach (var pool in poolDictionary.Values)
            {
                pool.Clear();
            }
            poolDictionary.Clear();
        }

        private class ObjectPool
        {
            private readonly Queue<GameObject> pooledObjects = new Queue<GameObject>();
            private readonly GameObject prefab;
            private readonly int maxSize;
            private int activeCount;
            private readonly GameObject poolContainer;

            public ObjectPool(GameObject prefab, int maxSize, Transform rootTransform, string poolName)
            {
                this.prefab = prefab;
                this.maxSize = maxSize;
                this.activeCount = 0;

                // 创建池容器
                poolContainer = new GameObject($"{poolName}Pool");
                poolContainer.transform.SetParent(rootTransform);
                poolContainer.transform.localPosition = Vector3.zero;
                poolContainer.transform.localRotation = Quaternion.identity;
                poolContainer.transform.localScale = Vector3.one;
            }

            public GameObject GetObject()
            {
                GameObject obj;
                if (pooledObjects.Count > 0)
                {
                    obj = pooledObjects.Dequeue();
                }
                else
                {
                    obj = CreateNewInstance();
                }
                activeCount++;
                return obj;
            }

            public void PushObject(GameObject obj)
            {
                if (obj == null) return;

                obj.SetActive(false);
                obj.transform.SetParent(poolContainer.transform);
                activeCount--;

                if (pooledObjects.Count < maxSize)
                {
                    pooledObjects.Enqueue(obj);
                }
                else
                {
                    UnityEngine.Object.Destroy(obj);
                }
            }

            public GameObject CreateNewInstance()
            {
                var obj = UnityEngine.Object.Instantiate(prefab);
                obj.name = prefab.name; // 移除(Clone)后缀
                obj.transform.SetParent(poolContainer.transform);
                return obj;
            }

            public void Clear()
            {
                while (pooledObjects.Count > 0)
                {
                    var obj = pooledObjects.Dequeue();
                    if (obj != null)
                    {
                        UnityEngine.Object.Destroy(obj);
                    }
                }
                if (poolContainer != null)
                {
                    UnityEngine.Object.Destroy(poolContainer);
                }
                activeCount = 0;
            }
        }
    }
}