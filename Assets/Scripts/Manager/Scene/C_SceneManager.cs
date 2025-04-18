using CFramework.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

public class C_SceneManager : BaseManager<C_SceneManager>
{
    private GameObject currentScene = null;

    /// <summary>
    /// 同步加载场景
    /// </summary>
    public void LoadScene(string name, UnityAction onComplete = null)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("[GameSceneManager] Scene name is null or empty.");
            return;
        }

        SceneManager.LoadScene(name);
        onComplete?.Invoke();
    }

    /// <summary>
    /// 异步加载场景
    /// </summary>
    public void LoadSceneAsync<T>(string name, UnityAction onComplete = null) where T : BaseScene
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("[GameSceneManager] Scene name is null or empty.");
            return;
        }

        UpdateDispatcher.Instance.StartCoroutine(LoadSceneAsyncCoroutine<T>(name, onComplete));
    }

    private IEnumerator LoadSceneAsyncCoroutine<T>(string name, UnityAction onComplete) where T : BaseScene
    {
        // 淡出过渡效果（如果有）
        // transition.FadeOut();

        // 退出当前场景
        if (currentScene != null)
        {
            currentScene.GetComponent<BaseScene>().ExitScene();
        }

        // 异步加载新场景
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);
        while (!asyncOperation.isDone)
        {
            // 触发加载进度事件（如果有）
            // EventCenter.GetInstance().EventTrigger("SceneLoadingProgress", asyncOperation.progress);
            yield return null;
        }

        // 初始化新场景
        currentScene = new GameObject("CurrentSceneManager");
        currentScene.AddComponent<T>().EnterScene();

        // 淡入过渡效果（如果有）
        // transition.FadeIn();

        // 调用完成回调
        onComplete?.Invoke();
    }

    public override void Initialize()
    {
        Debug.Log("[GameSceneManager] Initialized.");
    }

    public override void Shutdown()
    {
        Debug.Log("[GameSceneManager] Shutdown.");
    }
}
