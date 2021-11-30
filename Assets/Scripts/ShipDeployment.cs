using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDeployment : MonoBehaviour
{
    public float waveSpawnRate;
    float waveSpawnTimer;
    [SerializeField] int playerWaveSize;
    public int teamNumber;
    public GameObject fighter;
    public GameObject[] ships;
    [SerializeField]  List<GameObject> planets = new List<GameObject>();
    [SerializeField] ScoreTracker scoreTracker;
    GameObject squad;
    Color color;
    Quaternion startRotation;
    // Start is called before the first frame update
    void Start()
    {
        color = ColorSetter.Instance.GetColor(teamNumber);
        planets.AddRange(PlanetGenerator.Instance.planets);
        if (teamNumber != 0)
        {
            planets.Reverse();
        }
        else
        {
            InvokeRepeating("SpawnPlayerWave", 0.5f, waveSpawnRate);
        }
        startRotation = transform.rotation;
    }
    void SpawnPlayerWave()
    {
        SpawnWave(null, playerWaveSize);
    }
    // Update is called once per frame
    public void SpawnWave(List<GameObject> ships, int fighters)
    {
        for (int i = 0; i < fighters; i++)
        {
            SpawnFighter();
        }
        if (ships != null)
        {
            foreach(GameObject ship in ships)
            {
                ShipControl sc = ship.GetComponent<ShipControl>();

                sc.teamNumber = teamNumber;

                AutoControl ac = ship.GetComponent<AutoControl>();
                for (int x = 0; x < planets.Count; x++)
                {
                    ac.planets[x] = planets[x].GetComponent<ObjectiveScript>();
                }
                Instantiate(ship,
                    (Random.insideUnitCircle * 2) + (Vector2)transform.position,
                    transform.rotation);
            }
        }
    }
    void SpawnFrigate(int i)
    {
        GameObject ship = ships[i];
        ShipControl sc = ships[i].GetComponent<ShipControl>();
        sc.teamNumber = teamNumber;

        AutoControl ac = ship.GetComponent<AutoControl>();
        for (int x = 0; x < planets.Count; x++)
        {
            ac.planets[x] = planets[x].GetComponent<ObjectiveScript>();
        }
        Vector2 spawnPos = (Random.insideUnitCircle * 2f) + (Vector2)transform.position;

        Instantiate(ship,
            spawnPos,
            startRotation);
    }
    public void SpawnShip(int i)
    {
        if (i == -1)
        {
            for (int x = 0; x < playerWaveSize * 2; x++)
            {
                SpawnFighter();
            }
        }
        else
            SpawnFrigate(i);
    }
    void SpawnFighter()
    {

        FighterScript ship = Instantiate(
            fighter,
            (Random.insideUnitCircle * 2) + (Vector2)transform.position,
            startRotation).GetComponent<FighterScript>();
        ship.teamNumber = teamNumber;
        for (int i = 0; i < planets.Count; i++)
        {
            ship.planets[i] = planets[i].GetComponent<ObjectiveScript>();
        }
    }
    private void OnDestroy()
    {
        if (scoreTracker)
        {
            if (teamNumber == 0)
            {
                scoreTracker.LoseGame();
            }
        }
    }
}
