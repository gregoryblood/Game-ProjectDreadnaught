using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallMissileLauncher : WeaponScript
{
    public override void Fire(GameObject obj, ShipControl ship, GameObject currentTarget)
    {
        MissileScript missile = obj.GetComponent<MissileScript>();
        obj.GetComponent<SpriteRenderer>().color = ship.shipColor;
        missile.teamNumber = ship.teamNumber;
        missile.target = currentTarget;
    }
}
