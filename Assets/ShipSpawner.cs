using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShipSpawner : MonoBehaviour
{
    [SerializeField] float spawnRate = 5f;
    [SerializeField] float range = 50;
    [SerializeField] GameObject[] squads;
    float spawnTimer = 0;
    ColorSetter colorSetter;
    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = Time.time + 1f;
    }
    // Update is called once per frame
    void Update()
    {
        if (spawnTimer < Time.time)
        {
            spawnTimer = Time.time + spawnRate;
            SpawnShip();
        }
    }
    //[Server]
    void SpawnShip()
    {
        GameObject ship = Instantiate(
            squads[Random.Range(0, squads.Length)],
            new Vector2(Random.Range(-range, range), Random.Range(-range, range)),
            Quaternion.identity);
        ShipControl sc = ship.GetComponent<ShipControl>();
        int teamNum = Random.Range(0, 4);
        sc.teamNumber = teamNum;
        //NetworkServer.Spawn(ship, connectionToClient);

    }
}

