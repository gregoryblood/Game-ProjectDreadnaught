using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallMissileLauncher : WeaponScript
{
    public override void Fire()
    {
        MissileScript missile = obj.GetComponent<MissileScript>();
        obj.GetComponent<SpriteRenderer>().color = ship.shipColor;
        missile.teamNumber = ship.teamNumber;
        missile.target = currentTarget;
        missile.weapon = this;
        missile.shipLayer = shipLayer;
    }
}
