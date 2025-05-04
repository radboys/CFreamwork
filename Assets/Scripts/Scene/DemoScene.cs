using UnityEngine;

public class DemoScene : BaseScene
{

    public GameObject Zombie;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //InvokeRepeating("CreateZombie", 0, 3);
    }

    public GameObject CreateZombie()
    {
        GameObject zombie = Instantiate(Zombie);
        zombie.transform.position = new Vector3(0, 0, 0);
        return zombie;
    }
}
