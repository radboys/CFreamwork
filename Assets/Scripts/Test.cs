using UnityEngine;
using CFramework.Input;
using UnityEngine.InputSystem;
public class Test : MonoBehaviour
{
    private C_InputActionAsset input;

    private void Awake()
    {
        input = new C_InputActionAsset();
    }

    void Update()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 pos = Mouse.current.position.ReadValue();
            Debug.Log("持续获取鼠标位置：" + pos);
        }
    }
}