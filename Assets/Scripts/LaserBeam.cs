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
    [SerializeField] AudioClip shotSound;
    public int teamNumber;
    float fireTimer;
    float beamTimer;
    LineRenderer beam;
    FighterScript fighter;
    ShipMaster shipMaster;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        beam = GetComponent<LineRenderer>();
        beamTimer = fireTimer = Time.time;
        fighter = GetComponent<FighterScript>();
        shipMaster = ShipMaster.Instance;
        for (int i = 0; i < 4; i++)
        {
            if (i != teamNumber) 
                shipLayer |= (1 << LayerMask.NameToLayer("Ship" + i));
            if (i != teamNumber)
                shipLayer |= (1 << LayerMask.NameToLayer("Fighter" + i));
        }
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (fireTimer < Time.time)
        {
            fireTimer = Time.time + 1 / fireRate + Random.Range(-0.1f, 0.1f);
            beamTimer = Time.time + beamDuration;
            foreach (Collider2D target in Physics2D.OverlapCircleAll(transform.position, range, shipLayer))
            {
                if (shipMaster.DamageShip(target.GetInstanceID(), damage, target.transform.position))
                {
                    audioSource.PlayOneShot(shotSound);
                    beam.enabled = true;
                    beam.SetPosition(0, transform.position);
                    beam.SetPosition(1, target.transform.position);
                    fighter.attackTarget = target.gameObject;
                    break;
                }

            }

        }
        else if (beamTimer < Time.time)
        {
            beam.enabled = false;
        }

        
    }
}
