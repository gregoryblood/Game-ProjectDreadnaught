using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoControl : MonoBehaviour
{
    public Vector3 moveTarget;

    public ObjectiveScript[] planets = new ObjectiveScript[50];
    int currentObjNum = 0;
    int frameDif;
    ShipControl shipControl;
    Transform playerCapital;
    // Start is called before the first frame update
    void Start()
    {
        shipControl = GetComponent<ShipControl>();
        frameDif = Random.Range(0, 20);
        moveTarget = transform.position;
        currentObjNum = 1;
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
        if (shipControl.teamNumber != 0)
        {
            playerCapital = GameObject.Find("PlayerCapital").transform;
        }
    }

    private void Update()
    {
        if ((frameDif + Time.frameCount) % 20 != 0) return;
        if (shipControl.movedByPlayer)
            Destroy(this);
        //Objective movement
        if (!planets[currentObjNum])
        {
            if (playerCapital)
                MoveToNextObjective(playerCapital.position);
            return;
        }
        if (Vector3.Distance(transform.position, moveTarget) < 2f)
        {
            if (planets[currentObjNum].holdingTeam == shipControl.teamNumber)
            {
                currentObjNum++;
                if (planets[currentObjNum])
                    MoveToNextObjective(planets[currentObjNum].transform.position);
            }
        }

    }

    public void MoveToNextObjective(Vector2 position)
    {
        moveTarget = position + (Random.insideUnitCircle * (planets[currentObjNum] ? planets[currentObjNum].radius : 5f));
        shipControl.AutoMove(moveTarget);
    }
}
