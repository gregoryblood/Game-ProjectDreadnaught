using UnityEngine.UI;
using UnityEngine;
public class ShipDefence : MonoBehaviour
{
    public float health = 100;
    public int teamNumber;
    [SerializeField] string deathEffect;
    [SerializeField] Slider healthSlider;
    ObjectPooler objectPooler;
    public Color shipColor;
    float MaxHealth;
    // Start is called before the first frame update
    void Start()
    {
        objectPooler = ObjectPooler.Instance;
        MaxHealth = health;
        if (healthSlider)
            healthSlider.value = health / MaxHealth;
        //healthSlider.GetComponentInParent<Canvas>().worldCamera = Camera.main;
    }
    public void TakeDamage(float damage, Vector3 position)
    {
        health -= damage;
        //Ship Dies
        if (health < 1)
        {
            //TODO: Spawn big blasts
            GameObject myDebree = objectPooler.SpawnFromPool(deathEffect, transform.position, Quaternion.identity);
            ParticleSystem.EmissionModule emmision = myDebree.GetComponent<ParticleSystem>().emission;
            emmision.rateOverTime = 300f;
            Destroy(gameObject, 0f);
            //NetworkServer.Destroy(gameObject);
        }
        else //Ship doesnt die
        {
            if (healthSlider)
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
}

