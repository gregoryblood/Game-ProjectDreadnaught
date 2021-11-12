using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    public int numPlanets;
    [SerializeField] float range;
    [SerializeField] GameObject[] planets;
    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < numPlanets; i++)
        {
            Instantiate(planets[Random.Range(0, planets.Length)], Random.insideUnitCircle * range, Quaternion.identity);
        }
    }

}
