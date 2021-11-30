
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuScript : MonoBehaviour
{
    [SerializeField] GameObject MenuScreen;
    [SerializeField] GameObject ShowShips;
    [SerializeField] GameObject ShipMenu;
    [SerializeField] GameObject Abilities;
    [SerializeField] GameObject Cancel;
    public bool cancelAbility = false;
    [SerializeField] PlayerBank bank;

    public static MenuScript Instance;

    private void Awake()
    {
        Instance = this;
    }
    public void ToggleMenu()
    {
        MenuScreen.SetActive(!MenuScreen.activeSelf);
        Debug.Log("clicked");
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }
    public void ShipToggle()
    {
        ShowShips.SetActive(!ShowShips.activeSelf);
        ShipMenu.SetActive(!ShipMenu.activeSelf);
    }
    public void BuyShip(int ship)
    {
        bank.SpawnShip(ship);
    }

}
