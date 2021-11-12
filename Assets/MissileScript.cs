using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    [SerializeField] int damage = 20;
    [SerializeField] float speed = 10f;
    [SerializeField] float turnSpeed = 90f;
    [SerializeField] LayerMask shipLayer;
    public float teamNumber;
    public GameObject target;
    TrailRenderer trail;
    float actualTurnSpeed;
    ObjectPooler objectPooler;
    bool firstFrame = true;
    bool unPrimed = true;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        trail = GetComponentInChildren<TrailRenderer>();

    }
    void Explode()
    {
        objectPooler.SpawnFromPool("HitEffect", transform.position, transform.rotation);
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (!target)
        {
            Explode();
            return;
        }
        if (unPrimed)
            GetPrimed();
        else
        {
            if (timer < Time.time)
            {
                actualTurnSpeed += turnSpeed * actualTurnSpeed * Time.deltaTime;
            }
        }
        transform.Translate(Vector2.up * (!unPrimed ? speed : 1f) * Time.deltaTime);

        Vector2 dir = target.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, q, actualTurnSpeed * Time.deltaTime);
    }
    void GetPrimed()
    {
        if (firstFrame)
        {
            actualTurnSpeed = 1f;
            timer = Time.time + 1;
            firstFrame = false;
        }
        else if (timer < Time.time)
        {
            trail.enabled = true;
            unPrimed = false;
            timer = Time.time + 1.5f;
        }
    }
    private void FixedUpdate()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, 0.01f, shipLayer);
        if (col)
        {
            if (col.transform.root.gameObject.TryGetComponent(out ShipDefence ship))
            {
                if (ship.teamNumber != teamNumber)
                {
                    ship.TakeDamage(damage, transform.position);
                    Explode();
                }
            }

        }
    }
}
