using UnityEngine.EventSystems;
using UnityEngine;

public class BuyingButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] string name;
    [SerializeField] int number;
    PlayerBank bank;
    PlayerControl playerControl;
    MenuScript menuScript;
    bool cancelled;
    void Start()
    {
        bank = PlayerBank.Instance;
        playerControl = PlayerControl.Instance;
        cancelled = false;
        menuScript = MenuScript.Instance;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        playerControl.interactingElsewhere = true;
        Invoke("OpenToolTip",1f * Time.timeScale);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        playerControl.interactingElsewhere = false;
        CancelInvoke();
        if (!cancelled)
        {
            bank.SpawnShip(number);
            if (bank.coins == 0)
                menuScript.ShipToggle();
        }
            
        cancelled = false;

    }
    public void OnDrag(PointerEventData eventData)
    {
        CancelInvoke();
        cancelled = true;
    }
    void OpenToolTip()
    {
        cancelled = true;
        ToolTip.Instance.ShowToolTip(name);
    }
}
