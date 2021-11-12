using UnityEngine;
using System.Collections;

public class CannonShell : MonoBehaviour
{
    [SerializeField] int damage = 5;
    public float speed = 5f;
    [SerializeField] LayerMask shipLayer;
    public int teamNumber;
    public float lifetime;
    ObjectPooler objectPooler;
    float lifetimer;
    bool x = true;
    // Start is called before the first frame update
    private void Start()
    {
        objectPooler = ObjectPooler.Instance;
    }

    void Explode()
    {
        objectPooler.SpawnFromPool("HitEffect", transform.position, transform.rotation);
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (x)
        {
            lifetimer = Time.time + lifetime;
            x = false;
        }
        else if (lifetimer < Time.time)
        {
            Explode();
            return;
        }
        transform.Translate(Vector2.up * speed * Time.deltaTime);


    }
    private void FixedUpdate()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, 0.01f, shipLayer);
        if (!col)
            return;
        
        if (col.transform.parent.gameObject.TryGetComponent(out ShipDefence ship))
        {
            if (ship.teamNumber != teamNumber)
            {
                ship.TakeDamage(damage, transform.position);
                Explode();
            }
        }
    }

}
