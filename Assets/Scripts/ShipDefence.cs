using UnityEngine.UI;
using UnityEngine;
public class ShipDefence : MonoBehaviour
{
    public float health = 100;
    public int teamNumber;
    [SerializeField] string deathEffect;
    [SerializeField] Slider healthSlider;
    ObjectPooler objectPooler;
    ShipMaster shipMaster;
    [SerializeField] Collider2D hull;
    public Color shipColor;
    float MaxHealth;
    bool isSelected;
    // Start is called before the first frame update
    void Start()
    {
        if (hull)
        {
            ShipMaster.Instance.AddShip(hull);
            if (gameObject.CompareTag("Ship"))
            {
                hull.gameObject.layer = 11 + teamNumber;
            }
            else if (gameObject.CompareTag("Fighter"))
            {
                hull.gameObject.layer = 15 + teamNumber;
            }
        }
        objectPooler = ObjectPooler.Instance;
        shipMaster = ShipMaster.Instance;
        MaxHealth = health;
        if (healthSlider)
            healthSlider.value = health / MaxHealth;
        isSelected = false;
        //healthSlider.GetComponentInParent<Canvas>().worldCamera = Camera.main;
    }
    public void TakeDamage(float damage, Vector3 position)
    {
        health -= damage;
        //Ship Dies
        if (health < 1)
        {
            //TODO: Spawn big blasts
            objectPooler.SpawnFromPool(deathEffect, transform.position, Quaternion.identity);
            shipMaster.ships.Remove(hull.GetInstanceID());
            Destroy(gameObject, 0f);
            //NetworkServer.Destroy(gameObject);
        }
        else //Ship doesnt die
        {
            if (healthSlider && isSelected)
                healthSlider.value = health / MaxHealth;
            //Red hits
            objectPooler.SpawnFromPool("HitEffect", position, Quaternion.identity);
            if (damage > 10f || Random.Range(0f, 1f) > 0.8f)
            {
                GameObject myDebree = objectPooler.SpawnFromPool("ShipDebree", position, Quaternion.identity);
                ParticleSystem.MainModule settings = myDebree.GetComponent<ParticleSystem>().main;
                settings.startColor = shipColor;
            }
        }
        //Spawn blast at position
    }
    public void GotSelected()
    {
        isSelected = !isSelected;
        if (isSelected)
        {
            healthSlider.value = health / MaxHealth;
        }
    }
}

