using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WeaponScript : MonoBehaviour
{
    public float range = 10f;
    [SerializeField] string projectile;
    [SerializeField] float fireRate;
    [SerializeField] float reloadRate = 5f;
    [SerializeField] int shotsPerShot = 1;
    [SerializeField] int ammoMax = 3;
    [SerializeField] float spread;
    [SerializeField] AudioClip shotSound;
    [SerializeField] LayerMask shipLayer;
    int ammo = 0;
    float fireTimer = 0;
    float reloadTimer = 0;
    float scanTimer = 0;
    //public List<Collider2D> targets = new List<Collider2D>();
    GameObject currentTarget;
    ShipControl ship;
    AudioSource audioSource;
    ObjectPooler objectPooler;
    ShipDefence otherShip;
    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        audioSource = GetComponent<AudioSource>();
        ship = GetComponentInParent<ShipControl>();

        if (ship.maxAttackRange < range)
        {
            ship.maxAttackRange = range;
        }
        ammo = ammoMax;
        //InvokeRepeating("ReadTargets", 0, 0.1f);
    }
    void ReadTargets()
    {
        if (ship.targetShip) //If theres a target ship then attack it
        {
            if (range >= Vector3.Distance(transform.position, ship.targetShip.transform.position))
            {
                currentTarget = ship.targetShip;
                return;
            }
        }
        if (currentTarget) //If within range then no change;
        {
            if (range >= Vector3.Distance(transform.position, currentTarget.transform.position))
            {
                return;
            }
        }
        
        foreach (Collider2D target in Physics2D.OverlapCircleAll(transform.position, range, shipLayer))
        {
            if (otherShip = target.GetComponentInParent<ShipDefence>())
            {
                if (ship.teamNumber != otherShip.teamNumber)
                {
                    currentTarget = otherShip.gameObject;
                    break;
                }
            }
        }
    }
    void Update()
    {
        if (currentTarget != null)
        {
            if (ammo < ammoMax)
            {
                if (Time.time > reloadTimer)
                {
                    ammo = ammoMax;
                }
            }
            if (fireTimer < Time.time && ammo > 0)
            {
                audioSource.PlayOneShot(shotSound);
                fireTimer = Time.time + (1 / fireRate);

                for (int i = 0; i < shotsPerShot; i++)
                {
                    ammo--;

                    transform.up = (currentTarget.transform.position - transform.position);
                    transform.Rotate(Vector3.forward, Random.Range(-spread, spread));
                    GameObject bullet = objectPooler.SpawnFromPool(projectile, transform.position, transform.rotation);
                    Fire(bullet, ship, currentTarget);

                    if (ammo < 1) //Prevent shooting more than 0 ammo
                        break;
                }
                reloadTimer = Time.time + reloadRate;
            }
        }
        if (scanTimer < Time.time)
        {
            scanTimer = Time.time + 0.1f;
            ReadTargets();
        }
    }
    public virtual void Fire(GameObject obj, ShipControl ship, GameObject currentTarget)
    {

    }
}
