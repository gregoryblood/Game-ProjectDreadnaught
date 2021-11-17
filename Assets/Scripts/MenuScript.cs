using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuScript : MonoBehaviour
{
    [SerializeField] GameObject MenuScreen;
    [SerializeField] GameObject ShowShips;
    [SerializeField] GameObject ShipMenu;
    [SerializeField] PlayerBank bank;
    public void ToggleMenu()
    {
        MenuScreen.SetActive(!MenuScreen.activeSelf);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
