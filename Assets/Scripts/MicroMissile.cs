using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroMissile : MonoBehaviour
{
    [SerializeField] int damage = 20;
    [SerializeField] float blastRange = 5f;
    public LayerMask shipLayer;
    [SerializeField] Transform actualMissile;
    public float speed = 10f;
    public float teamNumber;
    ObjectPooler objectPooler;
    ShipMaster shipMaster;
    public bool justSpawned;
    public float lifeTime;
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
        Collider2D[] hits = Physics2D.OverlapCircleAll(actualMissile.position, blastRange, shipLayer);
        foreach (Collider2D hit in hits)
        {
            shipMaster.DamageShip(hit.GetInstanceID(), damage, transform.position);
        }
        objectPooler.SpawnFromPool("HitEffect", transform.position, transform.rotation);
        gameObject.SetActive(false);
    }

}
