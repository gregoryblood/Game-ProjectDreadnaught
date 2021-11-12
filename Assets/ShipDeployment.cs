using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShipDeployment : MonoBehaviour
{
    [SerializeField] int teamNumber;
    [SerializeField] GameObject fighter;
    [SerializeField] GameObject[] ships;
    [SerializeField] List<GameObject> planets = new List<GameObject>();
    Color color;
    LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        color = ColorSetter.Instance.GetColor(teamNumber);
        line = GetComponent<LineRenderer>();
        line.startColor = color/2f;
        line.endColor = color/2f;
        planets.AddRange(GameObject.FindGameObjectsWithTag("Planet"));
        planets = planets.OrderBy(
            x => Vector2.Distance(transform.position, x.transform.position)
           ).ToList();
        line.positionCount = planets.Count;
        for (int i = 0; i < planets.Count; i++)
        {
            //Make lines uneven
            float mod = teamNumber % 2 == 0 ? 0.1f : -0.1f;
            Vector3 p = new Vector3(
                planets[i].transform.position.x + (teamNumber * mod),
                planets[i].transform.position.y + (teamNumber * -mod),
                3); 
            line.SetPosition(i, p);
        }
        transform.up = planets[0].transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            for (int i = 0; i < 25; i++)
            {
                SpawnFighter();
            }
        }
    }
    void SpawnFighter()
    {
        FighterScript ship = Instantiate(fighter, transform.position, transform.rotation).GetComponent<FighterScript>();
        ship.teamNumber = teamNumber;
        for (int i = 0; i < planets.Count; i++)
        {
            ship.planets[i] = planets[i].GetComponent<ObjectiveScript>();
        }
        ship.MoveToNextObjective(planets[0].transform.position);
    }
}
