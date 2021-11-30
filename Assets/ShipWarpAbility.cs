using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipWarpAbility : AbilityDragHandler
{
    [SerializeField] GameObject shipToWarp;
    [SerializeField] int count = 1;
    public override void SpawnAbility(Vector2 target)
    {
        Quaternion rot = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        for (int i = 0; i < count; i++)
        {
            GameObject ship = Instantiate(shipToWarp, target, rot);
            if (ship.TryGetComponent(out ShipControl sc))
            {
                ship.GetComponent<ShipDefence>().health += 200;
                sc.teamNumber = PlayerControl.Instance.teamNumber;
                ship.GetComponent<AutoControl>().enabled = false;

            }
        }

    }
}
