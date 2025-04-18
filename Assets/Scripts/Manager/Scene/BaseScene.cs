using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BaseScene : MonoBehaviour
{
    /// <summary>
    /// 进入场景
    /// </summary>
    public virtual void EnterScene()
    {

    }

    /// <summary>
    /// 退出场景
    /// </summary>
    public virtual void ExitScene()
    {
        //UIManager.Instance.ClearPanel();
        Destroy(gameObject);
    }

}
