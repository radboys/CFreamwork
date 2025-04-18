
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BasePanel : MonoBehaviour
{
    // Start is called before the first frame update
    private Dictionary<string, List<UIBehaviour>> controllerDic = new Dictionary<string, List<UIBehaviour>>();
    void Start()
    {
        FindChildrenControl<Button>();

        // Update is called once per frame

    }
    public virtual void ShowMe()
    {

    }
    public virtual void HideMe()
    {

    }

    protected virtual void OnClick(string buttonName)
    {

    }

    protected T GetController<T>(string controllerName) where T : UIBehaviour
    {
        if (controllerDic.ContainsKey(controllerName))
        {
            for (int i = 0; i < controllerDic[controllerName].Count; i++)
            {
                if (controllerDic[controllerName][i] is T)
                {
                    return controllerDic[controllerName][i] as T;
                }
            }
        }
        return null;
    }
    private void FindChildrenControl<T>() where T : UIBehaviour
    {
        T[] controllers = this.GetComponentsInChildren<T>();
        string objName;
        for (int i = 0; i < controllers.Length; i++)
        {
            objName = controllers[i].name;
            if (controllerDic.ContainsKey(objName))
            {
                controllerDic[objName].Add(controllers[i]);
            }
            else
            {
                controllerDic.Add(controllers[i].gameObject.name, new List<UIBehaviour> { controllers[i] });
            }
            if (controllers[i] is Button)
            {
                (controllers[i] as Button).onClick.AddListener(() =>
                {
                    OnClick(objName);
                });
            }

        }
    }
}
