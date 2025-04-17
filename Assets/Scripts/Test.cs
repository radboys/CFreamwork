using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResourceLoader.Instance.LoadResourceAsync<PhysicsMaterial>("Smooth", (obj) =>
        {
            Debug.Log($"LoadResourceAsync: {obj.name}");
        });
    }
}
