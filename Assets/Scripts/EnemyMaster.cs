using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaster : MonoBehaviour
{

    ShipDeployment shipDeployer;
    GameObject[] ships;
    GameObject fighter;
    List<GameObject> squad = new List<GameObject>();
    [SerializeField] int waveSize;

    [SerializeField] int maxTokens;
    [SerializeField] int maxmaxTokens = 6;
    [SerializeField] float tokenIncreaseRate;
    float tokenIncreseTimer;
    int tokens = 0;
    // Start is called before the first frame update
    void Start()
    {
        shipDeployer = GetComponent<ShipDeployment>();
        ships = shipDeployer.ships;
        fighter = shipDeployer.fighter;
        InvokeRepeating("Squad", 0, shipDeployer.waveSpawnRate*2f);
        tokenIncreseTimer = Time.time + tokenIncreaseRate;
    }
    private void Update()
    {
        if (maxTokens == maxmaxTokens)
            return;
        if (tokenIncreseTimer < Time.time)
        {
            tokenIncreseTimer = Time.time + tokenIncreaseRate;
            tokenIncreaseRate *= 2;
            maxTokens++;
        }
    }
    void Squad()
    {
        int fighterCount = 0;
        tokens = maxTokens;

        while (tokens > 0)
        {
            int x = Random.Range(-1, ships.Length);
            if (x == -1 && tokens >= 1)
            {
                fighterCount++;
                tokens--;
            }
            else
            {
                ShipControl ship = ships[x].GetComponent<ShipControl>();
                if (tokens >= ship.cost) {
                    squad.Add(ships[x]);
                    tokens -= ship.cost;
                }
            }
        }
        shipDeployer.SpawnWave(squad, waveSize * (fighterCount + 1));
        squad.Clear();
    }

}
