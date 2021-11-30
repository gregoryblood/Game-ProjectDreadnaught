using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroMissile : MonoBehaviour
{
    [SerializeField] int damage = 20;
    [SerializeField] float blastRange = 5f;
    [SerializeField] string hitEffect;
    public LayerMask shipLayer;
    [SerializeField] Transform actualMissile;
    public float speed = 10f;
    public float teamNumber;
    ObjectPooler objectPooler;
    ShipMaster shipMaster;
    public bool justSpawned;
    public float lifeTime;
    public Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        shipMaster = ShipMaster.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (justSpawned)
        {
            lifeTime = Time.time + lifeTime;
            justSpawned = false;
            transform.up = (target - transform.position);
        }
        else if (lifeTime < Time.time)
        {
            Explode();
            return;
        }
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void Explode()
    {
        foreach (Collider2D hit in Physics2D.OverlapCircleAll(actualMissile.position, blastRange, shipLayer))
        {
            shipMaster.DamageShip(hit.GetInstanceID(), damage, transform.position);
        }
        objectPooler.SpawnFromPool(hitEffect, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }

}
