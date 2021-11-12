using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroMissile : MonoBehaviour
{
    [SerializeField] int damage = 20;
    [SerializeField] float blastRange = 5f;
    [SerializeField] LayerMask shipLayer;
    [SerializeField] Transform actualMissile;
    public float speed = 10f;
    public float teamNumber;
    ObjectPooler objectPooler;
    
    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        
    }

    public IEnumerator Explode(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        RaycastHit2D[] hits = Physics2D.CircleCastAll(actualMissile.position, blastRange, transform.up, blastRange, shipLayer);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.root.TryGetComponent(out ShipDefence ship))
            {
                if (ship.teamNumber != teamNumber)
                {
                    ship.TakeDamage(damage, ship.transform.position);
                }
            }
        }
        objectPooler.SpawnFromPool("HitEffect", transform.position, transform.rotation);
        gameObject.SetActive(false);
    }

}
