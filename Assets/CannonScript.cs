using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScript : WeaponScript
{
    public override void Fire(GameObject obj, ShipControl ship, GameObject currentTarget)
    {
        //Cosmetics
        obj.GetComponentInChildren<SpriteRenderer>().color = ship.shipColor;
        //Setting data
        float distance = Vector3.Distance(currentTarget.transform.position, transform.position);
        CannonShell shell = obj.GetComponent<CannonShell>();
        shell.lifetime = (distance / shell.speed) + Random.Range(0.2f, 0.6f);
        shell.teamNumber = ship.teamNumber;
    }
    
}
