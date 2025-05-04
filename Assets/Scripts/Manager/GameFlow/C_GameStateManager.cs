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
            Debug.Log("<color=green>[GameStateManager]</color> Initializing...");
            Debug.Log("<color=green>[GameStateManager]</color> Initialized.");
        }

        public override void Shutdown()
        {
            Debug.Log("<color=green>[GameStateManager]</color> Shutting down...");
            Debug.Log("<color=green>[GameStateManager]</color> Shutdown.");
        }
    }
}
