using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShipControl : MonoBehaviour
{
    public Color shipColor;
    public Color highlightColor;
    public int cost;
    public bool isSniper; //Stop ship if it shouldn't charge
    public float maxAttackRange = 5f; //Range of farthest weapon
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float turnSpeed = 10f;
    [SerializeField] float force = 10f;
    [SerializeField] float stopDistance = 0.5f;
    [SerializeField] AudioClip warpSound;
    public GameObject targetShip;
    GameObject followShip;
    Vector3 target;
    public int teamNumber = 0;
    bool isAttacking = false;
    bool hasOrder = false;
    SpriteRenderer sprite;
    Rigidbody2D rb;
    LineRenderer line;
    Vector3 currentPosition;
    Vector3 dir;
    Canvas canvas;
    ShipDefence shipDefence;
    public bool movedByPlayer;
    public bool weaponsFiring = false;
    RangeDrawer [] drawers;
    bool warping = true;
    AudioSource audioSource;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        canvas = GetComponentInChildren<Canvas>();
        audioSource = GetComponent<AudioSource>();
        canvas.enabled = false; ;
        line = GetComponent<LineRenderer>();
        drawers = GetComponentsInChildren<RangeDrawer>();
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);

        target = transform.position;
        ColorSetter cl = ColorSetter.Instance;
        shipColor = cl.GetColor(teamNumber);
        highlightColor = cl.GetHColor(teamNumber);
        sprite.color = shipColor;
        Color lineColor = shipColor;
        lineColor.a = 0.3f;
        line.startColor = lineColor;
        line.endColor = lineColor;
        line.enabled = false;
        shipDefence = GetComponent<ShipDefence>();
        shipDefence.teamNumber = teamNumber;
        shipDefence.shipColor = shipColor;
        movedByPlayer = false;
        shipDefence.hull.transform.localPosition -= (Vector3.up * 200f);
        warping = true;
    }

    private void FixedUpdate()
    {
        if (warping)
            return;
        if (followShip)
        {
            target = followShip.transform.position - currentPosition;
            line.SetPosition(1, followShip.transform.position);
            hasOrder = true;
        }
        if (!hasOrder) //Stop start spinning;
        {
            line.enabled = false;
            return;
        }
        if (target == null)
            return;
        //Calculate target
        line.SetPosition(0, transform.position);
        //Point to target
        dir = target - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, q, turnSpeed * Time.fixedDeltaTime);


        //Needs to move
        if (!isAttacking)
        {
            //Normal movement
            if (Vector3.Distance(transform.position, target) > stopDistance)
            {
                if (weaponsFiring && !movedByPlayer && isSniper)
                    return;
                UseThrusters();
            }
            else
            {
                hasOrder = false;
            }
        }
        else
        {
            if (targetShip != null)
            {
                target = targetShip.transform.position;
                if (Vector3.Distance(transform.position, targetShip.transform.position) > maxAttackRange - 1f)
                {
                    UseThrusters();
                }
                line.SetPosition(1, targetShip.transform.position);
            }
            else
            {
                isAttacking = false;
                hasOrder = false;
                target = transform.position;
            }
        }

    }
    private void Update()
    {
        if (warping)
        {
            UseWarper();
        }
    }
    void UseThrusters()
    {
        rb.AddForce(dir.normalized * force * Time.fixedDeltaTime);
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, isAttacking ? maxSpeed * 1.1f : maxSpeed);
        }
    }
    void UseWarper()
    {
        shipDefence.hull.transform.localPosition += (Vector3.up * 200f * Time.deltaTime);

        if (shipDefence.hull.transform.localPosition.y > 0.5f)
        {
            shipDefence.hull.transform.localPosition = Vector3.up * 0.5f;
            warping = false;
            audioSource.pitch = cost > 3 ? 2f: 3f;
            audioSource.volume = cost > 3 ? 0.3f : 0.2f;
            audioSource.PlayOneShot(warpSound);
            line.SetPosition(1, target);
        }
    }
    public void Move(Vector2 t)
    {

        target = t;
        hasOrder = true;
        isAttacking = false;
        followShip = null;
        if (!warping)
            line.SetPosition(1, t);

        if (teamNumber == 0)
            line.enabled = true;
        movedByPlayer = true;
    }
    public void AutoMove(Vector2 t)
    {
        Move(t);
        movedByPlayer = false;

    }
    public void Attack(GameObject ship)
    {
        targetShip = ship;
        target = ship.transform.position;
        isAttacking = true;
        hasOrder = true;
        followShip = null;
        if (teamNumber == 0)
            line.enabled = true;

    }
    public void Follow(GameObject ship)
    {
        followShip = ship;
        currentPosition = ship.transform.position - transform.position;
        if (currentPosition.magnitude > 8f)
        {
            currentPosition /= (currentPosition.magnitude / 5f);
        }
        hasOrder = true;
    }
    public void Select()
    {
        sprite.color = highlightColor;
        canvas.enabled = true;
        shipDefence.GotSelected();
        foreach (RangeDrawer drawer in drawers)
        {
            drawer.enabled = true;
        }
    }
    public void UnSelect()
    {
        sprite.color = shipColor;
        canvas.enabled = false;
        shipDefence.GotSelected();
        foreach (RangeDrawer drawer in drawers)
        {
            drawer.enabled = false;
        }
    }

}

