using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallMissileLauncher : WeaponScript
{
    public override void Fire(GameObject obj, ShipControl ship, GameObject currentTarget, WeaponScript weapon)
    {
        MissileScript missile = obj.GetComponent<MissileScript>();
        obj.GetComponent<SpriteRenderer>().color = ship.shipColor;
        missile.teamNumber = ship.teamNumber;
        missile.target = currentTarget;
        missile.weapon = weapon;
        missile.shipLayer = weapon.shipLayer;
    }
}
