using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterScript : MonoBehaviour
{

    [SerializeField] float turnSpeed = 10f;
    [SerializeField] float speed = 5;
    public int teamNumber;
    public Vector3 moveTarget;
    public GameObject attackTarget;
    public ObjectiveScript [] planets = new ObjectiveScript[50];
    int currentObjNum = 0;
    Rigidbody2D rb;
    int frameDif;
    Transform playerCapital;
    // Start is called before the first frame update
    void Start()
    {
        ColorSetter cl = ColorSetter.Instance;
        Color color = cl.GetColor(teamNumber);
        rb = GetComponent<Rigidbody2D>();

        GetComponent<LineRenderer>().startColor = color;
        GetComponent<LineRenderer>().endColor = color;
        GetComponentInChildren<SpriteRenderer>().color = color;
        ShipDefence sd = GetComponent<ShipDefence>();
        sd.teamNumber = teamNumber;
        sd.shipColor = color;
        GetComponent<LaserBeam>().teamNumber = teamNumber;
        frameDif = Random.Range(0, 20);
        currentObjNum = 0;
        float dist = Mathf.Infinity;
        for (int i = 0; i < planets.Length; i++)
        {
            if (!planets[i])
                break;
            float d = Vector3.Distance(transform.position, planets[i].transform.position);
            if (d < dist)
            {
                dist = d;
                currentObjNum = i;
            }
        }
        MoveToNextObjective(planets[currentObjNum].transform.position);
        if (teamNumber != 0)
        {
            playerCapital = GameObject.Find("PlayerCapital").transform;
        }
        turnSpeed -= Random.Range(0f, 0.6f);
    }
    /*    private void Update()
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
            //Point to target
            Vector2 dir = target - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, q, turnSpeed * Time.deltaTime);


        }*/
    // Update is called once per frame
    void FixedUpdate()
    {
        //Point to target
        Vector2 dir = (attackTarget ? attackTarget.transform.position : moveTarget) - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, q, turnSpeed * Time.deltaTime);

        rb.MovePosition(rb.position + ((Vector2)transform.up * speed * Time.fixedDeltaTime));

    }
    private void Update()
    {
        if ((frameDif + Time.frameCount) % 20 != 0) return;
        if (!attackTarget)
        {
            //Objective movement
            if (!planets[currentObjNum])
            {
                if (playerCapital)
                    MoveToNextObjective(playerCapital.position);
                return;
            }
            else if (Vector3.Distance(transform.position, moveTarget) < 2f)
            {
                if (planets[currentObjNum].holdingTeam == teamNumber)
                {
                    currentObjNum++;
                    if (planets[currentObjNum])
                        MoveToNextObjective(planets[currentObjNum].transform.position);
                }
            }
        }
    }

    public void MoveToNextObjective(Vector2 position)
    {
        moveTarget = position + (Random.insideUnitCircle * (planets[currentObjNum] ? planets[currentObjNum].radius : 5f));
    }
}
