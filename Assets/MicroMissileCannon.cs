using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroMissileCannon : WeaponScript
{
    [SerializeField] float spreadRate = 20;
    public override void Fire(GameObject obj, ShipControl ship, GameObject currentTarget)
    {
        //Cosmetics
        obj.GetComponentInChildren<SpriteRenderer>().color = ship.shipColor;
        //Setting data
        float distance = Vector3.Distance(currentTarget.transform.position, transform.position);
        if (obj.TryGetComponent(out MicroMissile missile))
        {
            missile.teamNumber = ship.teamNumber;
            missile.StartCoroutine(
                missile.Explode((distance / missile.speed)
                + Random.Range(-distance / spreadRate, distance / spreadRate)));
        }
    }
}
