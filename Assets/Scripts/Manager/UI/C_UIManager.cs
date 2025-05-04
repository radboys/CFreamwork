using System.Collections;
using System.Collections.Generic;
using CFramework.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

/// <summary>
/// UI层级枚举
/// - Bottom: 底层UI，通常用于背景或常驻UI
/// - Middle: 中层UI，用于主要交互界面
/// - Top: 顶层UI，用于弹窗或提示
/// - System: 系统层UI，用于系统级提示或全局UI
/// </summary>
public enum E_UI_Layer
{
    Bottom,
    Middle,
    Top,
    System,
}

/// <summary>
/// UI管理器
/// 功能：
/// 1. 管理所有显示的面板（加载、显示、隐藏、销毁）
/// 2. 提供外部接口用于操作UI面板
/// 3. 支持分层管理UI（Bottom/Middle/Top/System）
/// </summary>
public class C_UIManager : BaseManager<C_UIManager>
{
    // 存储所有已加载的面板，键为面板名称，值为面板对象
    private Dictionary<string, BasePanel> UIPanels = new();

    // UI层级父对象
    private Transform bottomLayer;    // 底层UI父对象
    private Transform middleLayer;    // 中层UI父对象
    private Transform topLayer;       // 顶层UI父对象
    private Transform SystemLayer;    // 系统层UI父对象

    // 过渡遮罩对象，用于场景切换或其他过渡效果
    public GameObject TransitionCover;

    // Canvas对象，用于管理UI的渲染层级
    private RectTransform canvas;

    /// <summary>
    /// 初始化方法（继承自BaseManager）
    /// </summary>
    public override void Initialize()
    {
        Debug.Log("<color=green>[C_UIManager]</color> Initializing...");
        Debug.Log("<color=green>[C_UIManager]</color> Initialized.");
    }

    /// <summary>
    /// 关闭方法（继承自BaseManager）
    /// </summary>
    public override void Shutdown()
    {
        Debug.Log("<color=green>[C_UIManager]</color> Shutting down...");
        Debug.Log("<color=green>[C_UIManager]</color> Shutdown.");
    }

    /// <summary>
    /// 构造函数（私有，确保单例模式）
    /// 初始化Canvas和EventSystem，并加载UI层级
    /// </summary>
    private C_UIManager()
    {
        // 异步加载Canvas
        C_ResourceManager.Instance.LoadResourceAsync<GameObject>("UI/Canvas", (obj) =>
        {
            if (obj == null)
            {
                Debug.LogError("[C_UIManager] Failed to load Canvas.");
                return;
            }

            // 实例化Canvas
            GameObject canvasObj = GameObject.Instantiate(obj);
            if (canvasObj == null)
            {
                Debug.LogError("[C_UIManager] Failed to instantiate Canvas.");
                return;
            }

            // 将Canvas转换为RectTransform并保存
            canvas = canvasObj.transform as RectTransform;
            if (canvas == null)
            {
                Debug.LogError("[C_UIManager] Canvas transform is not a RectTransform.");
                return;
            }

            // 确保Canvas在场景切换时不被销毁
            GameObject.DontDestroyOnLoad(canvasObj);

            // 加载各层级父对象
            bottomLayer = canvas.Find("Bottom");
            middleLayer = canvas.Find("Middle");
            topLayer = canvas.Find("Top");
            SystemLayer = canvas.Find("System");

            // 检查层级是否加载成功
            if (bottomLayer == null || middleLayer == null || topLayer == null || SystemLayer == null)
            {
                Debug.LogError("[C_UIManager] One or more UI layers are missing.");
                return;
            }

            // 加载过渡遮罩对象（可选）
            TransitionCover = canvas.Find("TransitionCover")?.gameObject;
            if (TransitionCover == null)
            {
                Debug.LogWarning("<color=green>[C_UIManager]</color> TransitionCover not found.");
            }
        });

        // 异步加载EventSystem
        C_ResourceManager.Instance.LoadResourceAsync<GameObject>("UI/EventSystem", (obj) =>
        {
            if (obj == null)
            {
                Debug.LogError("<color=green>[C_UIManager]</color> Failed to load EventSystem.");
                return;
            }

            // 实例化EventSystem
            GameObject eventSystemObj = GameObject.Instantiate(obj);
            if (eventSystemObj == null)
            {
                Debug.LogError("<color=green>[C_UIManager]</color> Failed to instantiate EventSystem.");
                return;
            }

            // 确保EventSystem在场景切换时不被销毁
            GameObject.DontDestroyOnLoad(eventSystemObj);
        });
    }

    /// <summary>
    /// 根据层级枚举获取对应的父对象
    /// </summary>
    /// <param name="layer">UI层级枚举</param>
    /// <returns>对应层级的父对象Transform</returns>
    public Transform GetLayerFather(E_UI_Layer layer)
    {
        switch (layer)
        {
            case E_UI_Layer.Bottom:
                return this.bottomLayer;
            case E_UI_Layer.Middle:
                return this.middleLayer;
            case E_UI_Layer.Top:
                return this.topLayer;
            case E_UI_Layer.System:
                return this.SystemLayer;
        }
        return null;
    }

    /// <summary>
    /// 显示指定名称的面板
    /// </summary>
    /// <typeparam name="T">面板脚本类型（必须继承自BasePanel）</typeparam>
    /// <param name="panelName">面板名称</param>
    /// <param name="layer">面板显示的层级（默认为Middle）</param>
    /// <param name="callBack">面板加载完成后的回调函数</param>
    public void ShowPanel<T>(string panelName, E_UI_Layer layer = E_UI_Layer.Middle, UnityAction<T> callBack = null) where T : BasePanel
    {
        if (string.IsNullOrEmpty(panelName))
        {
            Debug.LogError("<color=green>[C_UIManager]</color> Panel name is null or empty.");
            return;
        }

        // 检查面板是否已加载
        if (UIPanels.TryGetValue(panelName, out BasePanel panel))
        {
            panel.ShowMe();
            callBack?.Invoke(panel as T);
            return;
        }

        // 异步加载面板资源
        C_ResourceManager.Instance.LoadResourceAsync<GameObject>("UI/Panels/" + panelName, (obj) =>
        {
            if (obj == null)
            {
                Debug.LogError($"<color=green>[C_UIManager]</color> Failed to load panel: {panelName}");
                return;
            }

            // 获取对应层级的父对象
            Transform father = GetLayerFather(layer);
            if (father == null)
            {
                Debug.LogError($"<color=green>[C_UIManager]</color> Layer {layer} not found.");
                return;
            }

            // 设置面板的父对象和位置
            obj.transform.SetParent(father);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            // 如果是RectTransform，设置其锚点为全屏
            RectTransform rectTransform = obj.transform as RectTransform;
            if (rectTransform != null)
            {
                rectTransform.offsetMax = Vector2.zero;
                rectTransform.offsetMin = Vector2.zero;
            }

            // 获取面板脚本组件
            T panelComponent = obj.GetComponent<T>();
            if (panelComponent == null)
            {
                Debug.LogError($"<color=green>[C_UIManager]</color> Panel {panelName} does not have component of type {typeof(T)}.");
                return;
            }

            // 调用回调函数并显示面板
            callBack?.Invoke(panelComponent);
            panelComponent.ShowMe();
            UIPanels.Add(panelName, panelComponent);
        });
    }

    /// <summary>
    /// 隐藏指定名称的面板
    /// </summary>
    /// <param name="panelName">面板名称</param>
    public void HidePanel(string panelName)
    {
        if (string.IsNullOrEmpty(panelName))
        {
            Debug.LogError("<color=green>[C_UIManager]</color> Panel name is null or empty.");
            return;
        }

        if (UIPanels.TryGetValue(panelName, out BasePanel panel))
        {
            panel.HideMe();
        }
        else
        {
            Debug.LogWarning($"<color=green>[C_UIManager]</color> Panel {panelName} not found.");
        }
    }

    /// <summary>
    /// 获取指定名称的面板
    /// </summary>
    /// <typeparam name="T">面板脚本类型（必须继承自BasePanel）</typeparam>
    /// <param name="name">面板名称</param>
    /// <returns>面板对象，如果未找到则返回null</returns>
    public T GetPanel<T>(string name) where T : BasePanel
    {
        if (UIPanels.TryGetValue(name, out BasePanel panel))
        {
            return panel as T;
        }
        return null;
    }

    /// <summary>
    /// 为UI控件添加自定义事件监听
    /// </summary>
    /// <param name="control">控件对象</param>
    /// <param name="type">事件类型（如点击、拖拽等）</param>
    /// <param name="callBack">事件回调函数</param>
    public static void AddCustomEventListener(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> callBack)
    {
        if (control == null)
        {
            Debug.LogError("<color=green>[C_UIManager]</color> Control is null.");
            return;
        }

        // 获取或添加EventTrigger组件
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = control.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callBack);

        trigger.triggers.Add(entry);
    }

    public void ClearPanel()
    {
        if (UIPanels.Count != 0)
        {
            foreach (var kvp in UIPanels)
            {
                if (kvp.Value != null && kvp.Value.gameObject != null)
                {
                    GameObject.Destroy(UIPanels[kvp.Key].gameObject);
                }
            }
            UIPanels.Clear();
        }
    }
}
