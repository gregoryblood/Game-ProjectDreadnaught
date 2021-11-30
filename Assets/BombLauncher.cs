using UnityEngine;

public class BombLauncher : WeaponScript
{
    public override void Fire()
    {
        //Cosmetics
        obj.GetComponent<SpriteRenderer>().color = ship.shipColor;
        //Setting data
        float distance = Vector3.Distance(currentTarget.transform.position, transform.position);
        BombProjectile shell = obj.GetComponent<BombProjectile>();
        shell.shipLayer = shipLayer;
        shell.lifetime = (distance / shell.speed);
        shell.justSpawned = true;
        shell.teamNumber = ship.teamNumber;
    }
}
