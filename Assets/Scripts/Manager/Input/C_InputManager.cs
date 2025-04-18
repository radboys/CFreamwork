using UnityEngine;
using CFramework.Core;
using UnityEngine.InputSystem;

public class C_InputManager : BaseManager<C_InputManager>
{
    public override void Initialize()
    {
        Debug.Log("[C_InputManager] Initialized.");
    }

    public override void Shutdown()
    {
        Debug.Log("[C_InputManager] Shutdown.");
    }
}