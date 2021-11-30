using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScript : WeaponScript
{
    public override void Fire()
    {
        //Cosmetics
        obj.GetComponent<SpriteRenderer>().color = ship.shipColor;
        //Setting data
        //float distance = Vector3.Distance(currentTarget.transform.position, transform.position);
        CannonShell shell = obj.GetComponent<CannonShell>();
        shell.shipLayer = shipLayer;
        shell.lifetime = (range / shell.speed) + Random.Range(0.2f, 0.5f);
        shell.justSpawned = true;
        shell.teamNumber = ship.teamNumber;
    }
}
