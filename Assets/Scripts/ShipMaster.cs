using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMaster : MonoBehaviour
{
    public Dictionary<int, ShipDefence> ships = new Dictionary<int, ShipDefence>();

    public static ShipMaster Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void AddShip(Collider2D ship)
    {
        ships.Add(ship.GetInstanceID(), ship.GetComponentInParent<ShipDefence>());
    }
    public bool DamageShip(int id, float damage, Vector2 p)
    {
        if (!ships[id])
            return false;
        if (damage > -1)
            ships[id].TakeDamage(damage, p);
        return true;

    }

}
