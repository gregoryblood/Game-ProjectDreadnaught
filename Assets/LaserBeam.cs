using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField] float fireRate = 0.5f;
    [SerializeField] float beamDuration = 0.3f;
    [SerializeField] LayerMask shipLayer;
    [SerializeField] float range;
    [SerializeField] float damage;
    public int teamNumber;
    float fireTimer;
    float beamTimer;
    LineRenderer beam;

    // Start is called before the first frame update
    void Start()
    {
        beam = GetComponent<LineRenderer>();
        beamTimer = fireTimer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (fireTimer < Time.time)
        {
            fireTimer = Time.time + 1/fireRate + Random.Range(-0.1f, 0.1f);
            beamTimer = Time.time + beamDuration;
            Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, range, shipLayer);
            foreach (Collider2D target in targets)
            {
                if (target.transform.parent.gameObject.TryGetComponent(out ShipDefence otherShip))
                {
                    if (teamNumber != otherShip.teamNumber)
                    {
                        beam.enabled = true;
                        beam.SetPosition(0, transform.position);
                        beam.SetPosition(1, otherShip.transform.position);
                        otherShip.TakeDamage(damage, otherShip.transform.position);
                        break;
                    }
                }
            }
            
        }
        else if (beamTimer < Time.time)
        {
            beam.enabled = false;
        }
    }
}
