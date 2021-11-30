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
    ScoreTracker scoreTracker;
    List<Color> colors = new List<Color>();
    List<float> ships = new List<float>();
    int leaderTeam = -1;
    int currentTeam = -1;
    public int holdingTeam = -1; //Who owns complete control
    float totalShips = 0;
    bool maxCap = false;
    // Start is called before the first frame update
    private void Start()
    {
        GetComponent<CircleCollider2D>().radius = radius;
        foreach (Color color in ColorSetter.Instance.shipColors) {
            ships.Add(0);
            colors.Add(color);
        }

        scoreTracker = GameObject.Find("ScoreTracker").GetComponent<ScoreTracker>();
    }
    private void Update()
    {
        if (leaderTeam == -1)
            return;
        //Debug.Log(transform.name + "'s leading team is team " + leaderTeam);
        //Take points away if new team on field
        if (slider.value > 0f && leaderTeam != currentTeam)
        {
            holdingTeam = -1;
            //slider.value -= (ships[leaderTeam] - (totalShips - ships[leaderTeam])) * Time.deltaTime * capRate;
            slider.value -= Time.deltaTime * capRate;
            if (maxCap)
            {
                maxCap = false;
                if (holdingTeam == 0)
                {
                    scoreTracker.capturedPlanets--;
                }
            }
        }
        else
        {
            if (slider.value < 100f)
            {
                holdingTeam = -1;
                if (slider.value < 1f)
                {
                    currentTeam = leaderTeam;
                    fill.color = colors[currentTeam];
                }
                //slider.value += (ships[leaderTeam] - (totalShips - ships[leaderTeam])) * Time.deltaTime * capRate;
                slider.value += Time.deltaTime * capRate;
            }
            else
            {
                if (!maxCap)
                {
                    holdingTeam = leaderTeam;
                    maxCap = true;

                    if (holdingTeam == 0)
                    {
                        scoreTracker.capturedPlanets++;
                    }

                    

                }
                //SendOrders(currentTeam);
                return;
            }

        }
    }

    void CalcLeader()
    {
        float leaderShips = 0;
        leaderTeam = -1;
        for (int i = 0; i < ships.Count; i++)
        {
            if (ships[i] > 0)
            {
                if (ships[i] > leaderShips) //See who has the most ships
                {
                    leaderShips = ships[i];
                    leaderTeam = i;
                }
            }
        }
        if (leaderTeam != -1)
        {
            if (leaderShips < totalShips - ships[leaderTeam])
                leaderTeam = -1;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ShipDefence otherShip;

        if (otherShip = collision.GetComponentInParent<ShipDefence>())
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
        ShipDefence otherShip;
        if (otherShip = collision.GetComponentInParent<ShipDefence>())
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
