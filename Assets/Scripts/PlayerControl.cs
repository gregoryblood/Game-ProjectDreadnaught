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
    ShipControl enemySelected = null;
    LineRenderer line;
    [SerializeField] float selectingScaler = 1f;
    //Camera Settings
    Camera cam;
    [SerializeField] static float maxZoom = 30;
    [SerializeField] static float minZoom = 3;
    [SerializeField] float zoomSensitivity = 10f;
    [SerializeField] Transform background;
    //Camera Panning
    [SerializeField] float Boundary = 50;
    [SerializeField] float speed = 5;
    private int theScreenWidth;
    private int theScreenHeight;
    Vector3 mp; //MousePosition
    //Mobile
    bool isZooming = false;
    Vector2[] lastZoomPositions; // Touch mode only

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        cam = Camera.main;
        theScreenWidth = Screen.width;
        theScreenHeight = Screen.height;
        Application.targetFrameRate = 144;
        QualitySettings.vSyncCount = 1;
        InvokeRepeating("CheckScreen", 1f, 1f);
    }
    void CheckScreen()
    {
        if (theScreenWidth != Screen.width || theScreenHeight != Screen.height)
        {
            theScreenWidth = Screen.width;
            theScreenHeight = Screen.height;
        }
    }
    void HandleTouch()
    {
        //Mobile
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
            float prevMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMag = (touchZero.position - touchOne.position).magnitude;
            float difference = currentMag - prevMag;
            Zoom(difference * 0.01f);
            isZooming = true;
        }
    }
    void Zoom(float increment)
    {
        if (increment == 0)
            return;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - (increment * 1.3f), minZoom, maxZoom);
    }
    void HandleMouse()
    {
        //Camera zoom
        float fov = cam.orthographicSize;
        fov += Input.GetAxis("Mouse ScrollWheel") * -zoomSensitivity;
        fov = Mathf.Clamp(fov, minZoom, maxZoom);
        cam.orthographicSize = fov;
    }

    //[Client]
    void Update()
    {
        //if (!isLocalPlayer) return;


        if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
        {
            //Camera Zooming
            HandleTouch();
            if (isZooming)
            {
                if (Input.touchCount == 1)
                {
                    isZooming = false;
                    touchStart = cam.ScreenToWorldPoint(Input.GetTouch(0).position);
                }
            }
        }
        else
        {
            HandleMouse();
        }

        mp = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            if (currentShip)
            {
                currentShip.UnSelect();
                currentShip = null;
            }
            if (enemySelected)
            {
                enemySelected.UnSelect();
            }
            if (currentShip = Click())
            {
                currentShip.Select();
                if (currentShip.teamNumber != teamNumber)
                {
                    enemySelected = currentShip;
                    currentShip = null;
                }
                else
                {
                    line.enabled = true;
                }
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
                if (currentShip.teamNumber == teamNumber)
                {
                    if (ship != currentShip)
                    {
                        if (ship.teamNumber != teamNumber)
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
            }
            else
            {
                if (Vector3.Distance(cam.ScreenToWorldPoint(mp), currentShip.transform.position)-10f > 0.015f)
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
            AutoPanCamera();
            //Pan camera
            if (!currentShip)
            {
                if (isZooming)
                    return;
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
        else if (Input.GetKeyDown("1"))
        {
            teamNumber = 0;
        }
        else if (Input.GetKeyDown("2"))
        {
            teamNumber = 1;
        }
        else if (Input.GetKeyDown("3"))
        {
            teamNumber = 2;
        }
        else if (Input.GetKeyDown("4"))
        {
            teamNumber = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
    void AutoPanCamera()
    {
        if (isZooming)
            return;
        if (mp.x > theScreenWidth - (theScreenWidth * Boundary))
        {
            cam.transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else if (mp.x < 0 + (theScreenWidth * Boundary))
        {
            cam.transform.Translate(Vector3.right * -speed * Time.deltaTime);
        }
        if (mp.y > theScreenHeight - (theScreenWidth * Boundary))
        {
            cam.transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
        else if (mp.y < 0 + (theScreenWidth * Boundary))
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
                if (ship != currentShip)
                {
                    if ((ship.transform.position - cam.ScreenToWorldPoint(mp)).sqrMagnitude < dist)
                    {
                        closetShip = ship;
                        dist = (ship.transform.position - cam.ScreenToWorldPoint(mp)).sqrMagnitude;
                    }
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

