using UnityEngine;


public class ColorSetter : MonoBehaviour
{
    public static ColorSetter Instance;

    private void Awake()
    {
        Instance = this;
        /*GameObject[] ships = GameObject.FindGameObjectsWithTag("Ship");
        foreach (GameObject ship in ships)
        {
            ShipControl script = ship.GetComponent<ShipControl>();
            script.shipColor = shipColors[script.teamNumber];
            script.highlightColor = shiphColors[script.teamNumber];

        }*/
    }
    public Color GetColor(int teamNumber)
    {
        return shipColors[teamNumber];
    }
    public Color GetHColor(int teamNumber)
    {
        return shiphColors[teamNumber];
    }
    public Color[] shipColors;
    public Color[] shiphColors;

}
