using UnityEngine;

public class BombProjectile : MonoBehaviour
{
    [SerializeField] int damage = 5;
    [SerializeField] string hitEffect;
    [SerializeField] float blastRange;
    public float speed = 5f;
    public LayerMask shipLayer;
    public int teamNumber;
    public float lifetime;
    ObjectPooler objectPooler;
    ShipMaster shipMaster;
    float lifetimer;
    public bool justSpawned = true;
    // Start is called before the first frame update
    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
        shipMaster = ShipMaster.Instance;
    }

    void Explode()
    {
        foreach (Collider2D hit in Physics2D.OverlapCircleAll(transform.position, blastRange, shipLayer))
        {
            shipMaster.DamageShip(hit.GetInstanceID(), damage, transform.position);
        }
        for (int i = 0; i < blastRange * 8; i++)
        {
            objectPooler.SpawnFromPool(hitEffect, (Random.insideUnitCircle * blastRange) + (Vector2)transform.position, Quaternion.identity);
        }
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (justSpawned)
        {
            lifetimer = Time.time + lifetime;
            justSpawned = false;
        }
        else if (lifetimer < Time.time)
        {
            Explode();
            return;
        }
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
}
