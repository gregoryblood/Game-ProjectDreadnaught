using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveScript : MonoBehaviour

{
    public float radius = 5;
    [SerializeField] GameObject[] nextPlanets;
    [SerializeField] GameObject[] lastPlanets;
    [SerializeField] float capRate = 5f;
    [SerializeField] Slider slider;
    [SerializeField] Image fill;
    [SerializeField] LayerMask shipLayer;
    List<Color> colors = new List<Color>();
    List<float> ships = new List<float>();
    int leaderTeam = -1;
    int currentTeam = -1;
    public int holdingTeam = -1; //Who owns complete control
    float totalShips = 0;
    bool justCaptured = false;
    // Start is called before the first frame update
    private void Start()
    {
        GetComponent<CircleCollider2D>().radius = radius;
        foreach (Color color in ColorSetter.Instance.shipColors) {
            ships.Add(0);
            colors.Add(color);
        }
        justCaptured = true;
    }
    private void Update()
    {
        if (ships.Count == 0)
            return;
        else if (leaderTeam == -1)
            return;
        //Debug.Log(transform.name + "'s leading team is team " + leaderTeam);
        //Take points away if new team on field
        if (slider.value > 0f && leaderTeam != currentTeam)
        {
            //slider.value -= (ships[leaderTeam] - (totalShips - ships[leaderTeam])) * Time.deltaTime * capRate;
            slider.value -= Time.deltaTime * capRate;

        }
        else
        {
            if (slider.value < 100f)
            {
                holdingTeam = -1;
                if (slider.value < 1f)
                {
                    currentTeam = leaderTeam;
                    fill.color = colors[leaderTeam];
                }
                //slider.value += (ships[leaderTeam] - (totalShips - ships[leaderTeam])) * Time.deltaTime * capRate;
                slider.value += Time.deltaTime * capRate;
            }
            else
            {
                //justCaptured = true;//This is to not call send orders a lot
                holdingTeam = leaderTeam;
                //SendOrders(currentTeam);
                return;
            }

        }
    }
    void SendOrders(int teamThatWon)
    {
        //Pick next planet
        if (nextPlanets.Length == 0)
            return;
        GameObject nextPlanet = nextPlanets[Random.Range(0, nextPlanets.Length)];
        float nextRadius = nextPlanet.GetComponent<ObjectiveScript>().radius;
        foreach (Collider2D ship in Physics2D.OverlapCircleAll(transform.position, radius, shipLayer))
        {
            if (ship.transform.parent.TryGetComponent(out FighterScript fighter) )
            {
                if (fighter.teamNumber == teamThatWon)
                {
                    Vector2 randSpot = 
                        nextPlanet.transform.position + 
                        new Vector3(Random.Range(-nextRadius, nextRadius), Random.Range(-nextRadius, nextRadius), -2);
                    fighter.MoveToNextObjective(randSpot);
                }
            }
        }
    }
    void CalcLeader()
    {
        float leaderShips = -1;
        leaderTeam = -1;
        for (int i = 0; i < ships.Count; i++)
        {
            if (ships[i] > leaderShips && ships[i] > 0) //See who has the most ships
            {
                leaderShips = ships[i];
                leaderTeam = i;
            }
        }
        if (leaderShips < totalShips - ships[leaderTeam])
            leaderTeam = -1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.TryGetComponent(out ShipDefence otherShip))
        {
            if (otherShip.CompareTag("Fighter"))
            {
                ships[otherShip.teamNumber] += 0.05f;
                totalShips += 0.05f;
            }
            else
            {
                ships[otherShip.teamNumber]++;
                totalShips++;
            }

            CalcLeader();
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.TryGetComponent(out ShipDefence otherShip))
        {
            if (otherShip.CompareTag("Fighter"))
            {
                ships[otherShip.teamNumber] -= 0.05f;
                totalShips -= 0.05f;
            }
            else
            {
                ships[otherShip.teamNumber]--;
                totalShips--;
            }
            CalcLeader();
        }
    }
}
