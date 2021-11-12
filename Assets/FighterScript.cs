using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterScript : MonoBehaviour
{

    [SerializeField] float turnSpeed = 10f;
    [SerializeField] float speed = 5;
    public int teamNumber;
    public Vector3 target;
    public ObjectiveScript [] planets = new ObjectiveScript[50];
    int currentObjNum = 0;
    // Start is called before the first frame update
    void OnEnable()
    {
        ColorSetter cl = ColorSetter.Instance;
        Color color = cl.GetColor(teamNumber);
        GetComponent<LineRenderer>().startColor = color;
        GetComponent<LineRenderer>().endColor = color;
        GetComponentInChildren<SpriteRenderer>().color = color;
        ShipDefence sd = GetComponent<ShipDefence>();
        sd.teamNumber = teamNumber;
        sd.shipColor = color;
        GetComponent<LaserBeam>().teamNumber = teamNumber;
        InvokeRepeating("FindNextMove", Random.Range(0f, 0.5f), 2f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        //Point to target
        Vector2 dir = target - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, q, turnSpeed * Time.fixedDeltaTime);
    }
    void FindNextMove ()
    {
        if (planets[currentObjNum].holdingTeam == teamNumber)
        {
            currentObjNum++;
            if (planets[currentObjNum])
                MoveToNextObjective(planets[currentObjNum].transform.position);
            else
            {
                currentObjNum = 0;
            }
        }
    }
    public void MoveToNextObjective(Vector2 position)
    {

        target = position + (Random.insideUnitCircle * planets[currentObjNum].radius);
    }
}
