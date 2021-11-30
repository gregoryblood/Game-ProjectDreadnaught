using UnityEngine;
using UnityEngine.EventSystems;
public class AbilityDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    [SerializeField] MenuScript menu;
    [SerializeField] int abilityCost;
    [SerializeField] string name;
    PlayerControl playerControl;

    //LineDrawing Stuff
    float ThetaScale = 0.01f;
    public float radius = 5f;
    private int Size;
    private LineRenderer LineDrawer;
    private float Theta = 0f;
    Camera cam;
    bool cancelled;
    PlayerBank bank;
    void Start()
    {
        LineDrawer = GetComponent<LineRenderer>();
        cam = Camera.main;
        cancelled = false;
        Size = (int)((1f / ThetaScale) + 1f);
        LineDrawer.positionCount = Size;
        playerControl = PlayerControl.Instance;
        bank = PlayerBank.Instance;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Invoke("OpenToolTip", 1f * Time.timeScale);
        cancelled = false;
        if (bank.coins >= abilityCost)
        {
            playerControl.interactingElsewhere = true;
        }
        else cancelled = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) || Input.touchCount > 1)
        {
            cancelled = true;
            LineDrawer.enabled = false;
        }

    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!cancelled)
        {
            LineDrawer.enabled = true;
            DrawCircle();
        }
        CancelInvoke();


    }
    public void OnEndDrag(PointerEventData eventData)
    {
        playerControl.interactingElsewhere = false;
        LineDrawer.enabled = false;
        CancelInvoke();
        if (!cancelled)
        {
            bank.Exchange(abilityCost);
            SpawnAbility(cam.ScreenToWorldPoint(Input.mousePosition));
        }

    }
    public virtual void DrawCircle()
    {
        Theta = 0f;
        Vector2 mp = cam.ScreenToWorldPoint(Input.mousePosition);
        for (int i = 0; i < Size; i++)
        {
            Theta += (2.0f * Mathf.PI * ThetaScale);
            float x = radius * Mathf.Cos(Theta) + mp.x;
            float y = radius * Mathf.Sin(Theta) + mp.y; ;
            LineDrawer.SetPosition(i, new Vector3(x, y, 0));
        }
    }
    void OpenToolTip()
    {
        CancelInvoke();
        LineDrawer.enabled = false;
        ToolTip.Instance.ShowToolTip(name);
    }
    public virtual void SpawnAbility(Vector2 target)
    {

    }
}
