using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] int damage = 5;
    [SerializeField] LayerMask shipLayer;
    public float speed = 5f;
    [SerializeField] GameObject blastEffect;
    public float lifeTime = 5f;
    public float teamNumber;

    public List<Transform> bullets = new List<Transform>();
    // Update is called once per frame
    void LateUpdate()
    {
        foreach(Transform bullet in bullets)
        {
            bullet.Translate(Vector2.up * speed * Time.fixedDeltaTime);
            Collider2D col = Physics2D.OverlapCircle(bullet.transform.position, 0.1f, shipLayer);
            if (col)
            {
                ShipDefence ship = col.transform.root.gameObject.GetComponent<ShipDefence>();
                if (ship.teamNumber != teamNumber)
                {
                    ship.TakeDamage(damage, bullet.position);
                    Instantiate(blastEffect, bullet.position, bullet.rotation);
                    bullet.gameObject.SetActive(false);
                    bullets.Remove(bullet);
                }
            }

        }
    }
}
