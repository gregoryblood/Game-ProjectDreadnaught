using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerControl : MonoBehaviour
{
    [SerializeField] GameObject clickEffect;
    public int teamNumber = 0;
    [SerializeField] LayerMask shipLayer;
    Vector3 touchStart;
    ShipControl currentShip = null;
    LineRenderer line;
    [SerializeField] float selectingScaler = 1f;
    //Camera Settings
    Camera cam;
    [SerializeField] float maxZoom = 20;
    [SerializeField] float minZoom = 3;
    [SerializeField] float zoomSensitivity = 10f;
    [SerializeField] float selectSensitivity = 0.5f;
    [SerializeField] Transform background;
    //Camera Panning
    [SerializeField] float Boundary = 50;
    [SerializeField] float speed = 5;
    private int theScreenWidth;
    private int theScreenHeight;
    Vector3 mp; //MousePosition
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        cam = Camera.main;
        theScreenWidth = Screen.width;
        theScreenHeight = Screen.height;
    }
    //[Client]
    void Update()
    {
        //if (!isLocalPlayer) return;
        //Camera zoom
        float fov = cam.orthographicSize;
        fov += Input.GetAxis("Mouse ScrollWheel") * -zoomSensitivity;
        fov = Mathf.Clamp(fov, minZoom, maxZoom);
        cam.orthographicSize = fov;
        mp = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            if (currentShip)
            {
                currentShip.UnSelect();
            }
            if (currentShip = Click())
            {
                currentShip.Select();
                line.enabled = true;
            }
            touchStart = cam.ScreenToWorldPoint(mp);
        }
        else if (Input.GetMouseButtonUp(0) && currentShip)
        {
            line.enabled = false;
            ShipControl ship;
            //Attack
            if (ship = Click())
            {
                //If you have a friendly ship selected
                if (true)//if (currentShip.teamNumber == teamNumber)
                {
                    if (ship.teamNumber != teamNumber && ship != currentShip)
                    {
                        currentShip.Attack(ship.gameObject);
                        currentShip.UnSelect();
                        currentShip = null;
                    }
                    else
                    {
                        currentShip.Follow(ship.gameObject);
                        currentShip.UnSelect();
                        currentShip = null;
                    }
                }
            }
            else
            {
                if (true)//if (currentShip.teamNumber == teamNumber)
                {
                    MoveShip(currentShip, cam.ScreenToWorldPoint(mp));
                    currentShip.UnSelect();
                    currentShip = null;
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            //Auto Panning camera if mouse at edge of screen
            PanCamera();
            //Pan camera
            if (!currentShip)
            {
                Vector3 direction = touchStart - cam.ScreenToWorldPoint(mp);
                cam.transform.position += direction;
                if (background) background.position = cam.transform.position/5f;
            }
            //Moving ship
            else
            {
                line.SetPosition(0, currentShip.transform.position);
                line.SetPosition(1, (Vector2)cam.ScreenToWorldPoint(mp));
            }
        }
        else if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (Input.GetKeyDown("="))
        {
            Time.timeScale += 0.25f;
        }
        else if (Input.GetKeyDown("-"))
        {
            Time.timeScale -= 0.25f;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
    void PanCamera()
    {
        if (mp.x > theScreenWidth - Boundary)
        {
            cam.transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else if (mp.x < 0 + Boundary)
        {
            cam.transform.Translate(Vector3.right * -speed * Time.deltaTime);
        }
        if (mp.y > theScreenHeight - Boundary)
        {
            cam.transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        else if (mp.y < 0 + Boundary)
        {
            cam.transform.Translate(Vector3.up * -speed * Time.deltaTime);
        }
    }
    ShipControl Click()
    {
        ShipControl ship = null;
        ShipControl closetShip = null;
        float dist = 1000f;
        //Find closest ship to pointer
        foreach (Collider2D hull in Physics2D.OverlapCircleAll(
            cam.ScreenToWorldPoint(mp), cam.orthographicSize * selectingScaler, shipLayer))
        {
            if (ship = hull.GetComponentInParent<ShipControl>())
            {
                if ((ship.transform.position - cam.ScreenToWorldPoint(mp)).sqrMagnitude < dist)
                {
                    closetShip = ship;
                    dist = (ship.transform.position - mp).sqrMagnitude;
                }
            }
        }
        if (closetShip) return closetShip;
        else return null;
    }
    //[Command]
    void MoveShip(ShipControl ship, Vector2 pos)
    {
        ship.Move(pos);
    }
}

