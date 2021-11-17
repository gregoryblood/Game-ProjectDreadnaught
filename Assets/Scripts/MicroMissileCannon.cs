using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroMissileCannon : WeaponScript
{
    [SerializeField] float spreadRate = 20;
    public override void Fire(GameObject obj, ShipControl ship, GameObject currentTarget, WeaponScript weapon)
    {
        //Cosmetics
        obj.GetComponentInChildren<SpriteRenderer>().color = ship.shipColor;
        //Setting data
        float distance = Vector3.Distance(currentTarget.transform.position, transform.position);
        if (obj.TryGetComponent(out MicroMissile missile))
        {
            missile.teamNumber = ship.teamNumber;
            missile.lifeTime = (distance / missile.speed)
                + Random.Range(-(distance / spreadRate)/2f, distance / spreadRate);
            missile.justSpawned = true;
            missile.shipLayer = weapon.shipLayer;
        }
    }
}
