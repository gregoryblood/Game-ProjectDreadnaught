using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    public EnemyAI masterBrain;
    public ShipControl shipControl;
    // Start is called before the first frame update
    void Start()
    {
        FindNewEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FindNewEnemy()
    {
        foreach (GameObject ship in masterBrain.enemies)
        {
            float dist = Vector2.Distance(ship.transform.position, transform.position);
            float minDist = Mathf.Infinity;
            if (dist < minDist)
            {
                shipControl.Attack(ship);
            }
        }
    }
}
