using UnityEngine;
using CFramework.Core;

namespace CFramework.Managers
{
    public class C_GameStateManager : BaseManager<C_GameStateManager>
    {
        // 私有构造函数，确保通过 BaseManager<Create>() 进行懒加载
        private C_GameStateManager() { }
        public override void Initialize()
        {
            Debug.Log("[GameStateManager] Initializing...");

            Debug.Log("[GameStateManager] Initialized.");
        }

        public override void Shutdown()
        {
            Debug.Log("[GameStateManager] Shutting down...");

            Debug.Log("[GameStateManager] Shutdown.");
        }
    }
}
