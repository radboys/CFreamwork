using UnityEngine;
using CFramework.Core;
using UnityEngine.InputSystem;

public class C_InputManager : BaseManager<C_InputManager>
{
    public override void Initialize()
    {
        Debug.Log("<color=green>[C_InputManager]</color> Initialized.");
    }

    public override void Shutdown()
    {
        Debug.Log("<color=green>[C_InputManager]</color> Shutdown.");
    }
}