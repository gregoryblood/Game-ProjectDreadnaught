using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class PlayerBank : MonoBehaviour
{
    [SerializeField] int coins;
    [SerializeField] float coinRate;
    float coinTimer;
    ShipDeployment spawner;
    GameObject[] ships;
    [SerializeField] TMP_Text coinText;
    [SerializeField] Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject deployer in GameObject.FindGameObjectsWithTag("Deployer"))
        {
            if (deployer.GetComponent<ShipDeployment>().teamNumber == 0)
            {
                spawner = deployer.GetComponent<ShipDeployment>();
            }
        }
        coinTimer = Time.time + coinRate;
        coinText.text = coins.ToString();
        slider.value = 0;
        ships = spawner.ships;
    }
    private void Update()
    {
        if (coinTimer < Time.time)
        {
            coinTimer = Time.time + coinRate;
            slider.value = 0;
            coins++;
            coinText.text = coins.ToString();
        }
        slider.value += (1f / coinRate) * Time.deltaTime ;
    }
    public void SpawnShip(int i)
    {
        if (i == -1 && coins >= 1) {
            coins--;
            spawner.SpawnShip(i);
        }
        else if (i > -1)
        {
            ShipControl sc = ships[i].GetComponent<ShipControl>();
            if (i >= 0 && i < ships.Length && coins >= sc.cost)
            {
                coins -= sc.cost;
                spawner.SpawnShip(i);
            }
         }     
        coinText.text = coins.ToString();
        
    }
}
