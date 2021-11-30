using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroMissileCannon : WeaponScript
{
    [SerializeField] float accuracy = 5;
    public override void Fire()
    {
        //Cosmetics
        obj.GetComponentInChildren<SpriteRenderer>().color = ship.shipColor;
        //Setting data
        Vector3 actualTarget = currentTarget.transform.position + (Vector3)Random.insideUnitCircle * accuracy;
        float distance = Vector3.Distance(actualTarget, transform.position);
        if (obj.TryGetComponent(out MicroMissile missile))
        {
            missile.teamNumber = ship.teamNumber;
            missile.lifeTime = (distance / missile.speed);
            missile.justSpawned = true;
            missile.shipLayer = shipLayer;
            missile.target = actualTarget;
        }
    }
}
