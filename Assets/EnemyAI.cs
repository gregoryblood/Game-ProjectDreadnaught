using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] int teamNumber = 1;
    public List<ShipControl> ships = new List<ShipControl>();
    public List<GameObject> enemies = new List<GameObject>();
    List<GameObject> objs = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] allShips = GameObject.FindGameObjectsWithTag("Ship");
        foreach(GameObject ship in allShips)
        {
            ShipControl x = ship.GetComponent<ShipControl>();
            if (x.teamNumber == teamNumber)
            {
                ships.Add(x);
                ship.AddComponent<EnemyBrain>();
                ship.GetComponent<EnemyBrain>().masterBrain = this;
                ship.GetComponent<EnemyBrain>().shipControl = x;
            }
            else
            {
                enemies.Add(ship);
            }
        }
        GameObject[] allObjs = GameObject.FindGameObjectsWithTag("Obj");
        foreach (GameObject obj in allObjs)
        {
            objs.Add(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
