using UnityEngine;

public class GarlicController : WeaponController
{
    

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Attack();
        GameObject spawnedGarlic = Instantiate(prefab);
        spawnedGarlic.transform.position = transform.position;
        spawnedGarlic.transform.parent = transform;
    }
}
