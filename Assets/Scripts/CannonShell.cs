using UnityEngine;
using System.Collections;

public class CannonShell : MonoBehaviour
{
    [SerializeField] int damage = 5;
    public float speed = 5f;
    public LayerMask shipLayer;
    public int teamNumber;
    public float lifetime;
    ObjectPooler objectPooler;
    ShipMaster shipMaster;
    float lifetimer;
    public bool piercing = false;
    public bool justSpawned = true;
    // Start is called before the first frame update
    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
        shipMaster = ShipMaster.Instance;
    }

    void Explode()
    {
        objectPooler.SpawnFromPool("HitEffect", transform.position, transform.rotation);
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
        if (Time.frameCount % 2 == 0)
        {
            Collider2D col = Physics2D.OverlapCircle(transform.position, 0.01f, shipLayer);
            if (!col)
                return;

            if (shipMaster.DamageShip(col.GetInstanceID(), damage, transform.position))
            {
                if (!piercing)
                    Explode();
            }
        }


    }
}
