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
    public LayerMask frigateLayer;
    public LayerMask fighterLayer;
    public LayerMask shipLayer;
    int ammo = 0;
    float fireTimer = 0;
    float reloadTimer = 0;
    float scanTimer = 0;
    //public List<Collider2D> targets = new List<Collider2D>();
    public GameObject currentTarget;
    ShipControl ship;
    AudioSource audioSource;
    ObjectPooler objectPooler;
    ShipMaster shipMaster;
    bool targetIsFighter;
    // Start is called before the first frame update
    void Start()
    {
        shipMaster = ShipMaster.Instance;
        objectPooler = ObjectPooler.Instance;
        audioSource = GetComponent<AudioSource>();
        ship = GetComponentInParent<ShipControl>();

        if (ship.maxAttackRange < range)
        {
            ship.maxAttackRange = range;
        }
        ammo = ammoMax;
        for (int i = 0; i < 4; i++)
        {
            if (i != ship.teamNumber)
            {
                frigateLayer |= (1 << LayerMask.NameToLayer("Ship" + i));
                fighterLayer |= (1 << LayerMask.NameToLayer("Fighter" + i));
                shipLayer |= (1 << LayerMask.NameToLayer("Fighter" + i));
                shipLayer |= (1 << LayerMask.NameToLayer("Ship" + i));
            }

        }
        //InvokeRepeating("ReadTargets", 0, 0.1f);
    }
    void ReadTargets()
    {
        if (ship.targetShip) //If theres a target ship then attack it
        {
            if (range >= Vector3.Distance(transform.position, ship.targetShip.transform.position))
            {
                //If in range
                targetIsFighter = false;
                currentTarget = ship.targetShip;
                return;
            }
        }
        if (targetIsFighter)
            LookForFrigates();
        if (currentTarget) //If within range then no change;
        {
            //If in range
            if (range >= Vector3.Distance(transform.position, currentTarget.transform.position))
            {
                return;
            }
            currentTarget = null;
        }
        if (!targetIsFighter) //Prevent checking twice
            LookForFrigates();
        if (currentTarget)
            return;
        LookForFighters();
    }
    void LookForFrigates()
    {
        //Look for big ships
        foreach (Collider2D target in Physics2D.OverlapCircleAll(transform.position, range, frigateLayer))
        {
            if (shipMaster.DamageShip(target.GetInstanceID(), -1, target.transform.position))
            {
                targetIsFighter = false;
                currentTarget = target.transform.parent.gameObject;
                return;
            }
        }
    }
    void LookForFighters()
    {
        //Look for little ships
        foreach (Collider2D target in Physics2D.OverlapCircleAll(transform.position, range, fighterLayer))
        {
            if (shipMaster.DamageShip(target.GetInstanceID(), -1, target.transform.position))
            {
                currentTarget = target.transform.parent.gameObject;
                targetIsFighter = true;
                return;
            }
        }
    }
    void LateUpdate()
    {
        if (scanTimer < Time.time)
        {
            ship.weaponsFiring = false;
            scanTimer = Time.time + 0.2f;
            ReadTargets();
        }
        if (currentTarget != null)
        {
            ship.weaponsFiring = true;

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
                    if (!currentTarget)
                        return;
                    transform.up = (currentTarget.transform.position - transform.position);
                    transform.Rotate(Vector3.forward, Random.Range(-spread, spread));
                    GameObject bullet = objectPooler.SpawnFromPool(projectile, transform.position, transform.rotation);
                    Fire(bullet, ship, currentTarget, this);

                    if (ammo < 1) //Prevent shooting more than 0 ammo
                        break;
                }
                reloadTimer = Time.time + reloadRate;
            }
        }
    }
    public virtual void Fire(GameObject obj, ShipControl ship, GameObject currentTarget, WeaponScript weapon)
    {
        
    }
}
