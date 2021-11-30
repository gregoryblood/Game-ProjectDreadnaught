using UnityEngine;

public class MicroBarrageAbility : AbilityDragHandler
{
    public WeaponScript missileLauncher;
    // Start is called before the first frame update
    public override void SpawnAbility(Vector2 target)
    {
        GameObject objToSpawn = new GameObject("Temp");
        objToSpawn.transform.position = target;
        missileLauncher.currentTarget = objToSpawn;
        Destroy(objToSpawn, 30);
    }
}
