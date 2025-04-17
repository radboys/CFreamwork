using System;
using System.Collections.Generic;
using UnityEngine;
using CFramework.Core;

namespace CFramework.Managers
{
    /// <summary>
    /// 对象池管理器：管理游戏对象的复用
    /// </summary>
    public class ObjectPoolManager : BaseManager<ObjectPoolManager>
    {
        // 私有构造，确保通过 BaseManager<Create>() 进行懒加载
        private ObjectPoolManager() { }

        public override void Initialize()
        {
            Debug.Log("[ObjectPoolManager] Initializing...");
            //_poolDictionary.Clear();
            Debug.Log("[ObjectPoolManager] Initialized.");
        }

        public override void Shutdown()
        {
            Debug.Log("[ObjectPoolManager] Shutting down...");
            //ClearAllPools();
            Debug.Log("[ObjectPoolManager] Shutdown.");
        }

    }
}