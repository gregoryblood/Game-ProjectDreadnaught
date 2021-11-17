using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    public static PlanetGenerator Instance;
    public int numPlanets;
    [SerializeField] bool randomPlanets;
    [SerializeField] float range;
    [SerializeField] GameObject[] planetsPrefabs;
    public List<GameObject> planets = new List<GameObject>();
    [SerializeField] Transform playerStart;

    LineRenderer line;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        if (randomPlanets)
        {
            numPlanets += Random.Range(-1, 2);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Deployer"))
        {
            planets.Add(Instantiate(planetsPrefabs[Random.Range(0, planetsPrefabs.Length)],
                (Vector2)obj.transform.position + Random.insideUnitCircle * 3f, Quaternion.identity));
        }
        for (int i = 0; i < numPlanets; i++)
        {
            planets.Add(Instantiate(planetsPrefabs[Random.Range(0, planetsPrefabs.Length)],
                Random.insideUnitCircle * range, Quaternion.identity));
        }
        FindStartingPlanet();
        SortByClosest();
        //MakeRing();
        line = GetComponent<LineRenderer>();
        line.positionCount = planets.Count;
        for (int i = 0; i < planets.Count; i++)
        {
            line.SetPosition(i, planets[i].transform.position);
        }
    }
    void MakeRing()
    {

        Vector3 avg = Vector3.zero;
        foreach (GameObject pos in planets)
        {
            avg += pos.transform.position;
        }
        avg /= planets.Count;
        Debug.Log(avg);
        GameObject temp;
        for (int i = 0; i < planets.Count - 1; i++)
        {
            for (int j = 0; j < planets.Count - i - 1; j++)
            {
                if ((planets[j].transform.position - avg).sqrMagnitude < (planets[j + 1].transform.position - avg).sqrMagnitude)
                {
                    //Swap with planet were checking
                    temp = planets[j + 1];
                    planets[j + 1] = planets[j];
                    planets[j] = temp;
                }

            }

        }
    }
    void SortByClosest()
    {

        GameObject temp;
        int[] walked = new int[planets.Count - 1];
        for (int i = 0; i < walked.Length; i++)
        {
            walked[i] = -1;
        }
        walked[0] = 0;
        for (int i = 0; i < planets.Count - 1; i++)
        {
            int closest = -1;
            float dist = Mathf.Infinity;
            walked[i] = i;

            //Find next closest planet
            for (int j = i + 1; j < planets.Count; j++)
            {
                //If we havent checked this planet already
                if (!HasWalked(walked, j))
                {
                    float x = (planets[i].transform.position - planets[j].transform.position).sqrMagnitude;
                    if (x < dist)
                    {
                        closest = j;
                        dist = x;
                    }
                }
            }
            if (closest >= 0)
            {
                //Swap with planet were checking
                temp = planets[i + 1];
                planets[i + 1] = planets[closest];
                planets[closest] = temp;
            }
        }
    }
    bool HasWalked(int[] walked, int y)
    {

        for (int x = 0; x < walked.Length; x++)
        {
            if (walked[x] != -1)
            {
                if (walked[x] == y)
                {
                    return true;
                }
            }
            else break;
        }
        return false;
    }
    void FindStartingPlanet()
    {
        float dist = Mathf.Infinity;
        int closest = -1;
        GameObject temp;
        //Find Closest Planet
        for (int i = 0; i < planets.Count; i++)
        {
            float x = (playerStart.position - planets[i].transform.position).sqrMagnitude;
            if (x < dist)
            {
                closest = i;
                dist = x;
            }
        }
        //Swap with planet were checking
        temp = planets[0];
        planets[0] = planets[closest];
        planets[closest] = temp;

    }

}
