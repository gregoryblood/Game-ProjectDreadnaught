using UnityEngine;
using UnityEngine.UI;
public class ShipControl : MonoBehaviour
{
    public Color shipColor;
    public Color highlightColor;
    public float maxAttackRange = 5f; //Range of farthest weapon
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float turnSpeed = 10f;
    [SerializeField] float force = 10f;
    [SerializeField] float stopDistance = 0.5f;
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

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        canvas = GetComponentInChildren<Canvas>();
        canvas.enabled = false; ;
        line = GetComponent<LineRenderer>();

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
        shipDefence = GetComponent<ShipDefence>();
        shipDefence.teamNumber = teamNumber;
        shipDefence.shipColor = shipColor;
    }

    private void Update()
    {
        //Calculate target
        line.SetPosition(0, transform.position);
    }
    private void FixedUpdate()
    {
        if (followShip)
        {
            target = followShip.transform.position - currentPosition;
            line.SetPosition(1, followShip.transform.position);
            hasOrder = true;
        }

        if (!hasOrder) //Stop start spinning;
            return;
        if (target == null)
            return;
        //Point to target
        dir = target - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, q, turnSpeed * Time.fixedDeltaTime);


        //Needs to move
        if (!isAttacking)
        {

            if (Vector3.Distance(transform.position, target) > stopDistance)
            {
                UseThrusters();
            }
            else
            {
                hasOrder = false;
            }
        }
        else
        {
            if (targetShip)
            {
                target = targetShip.transform.position;

                if (Vector3.Distance(transform.position, target) > maxAttackRange - 1f)
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
                line.SetPosition(0, transform.position);
                line.SetPosition(1, transform.position);
            }

        }

    }
    void UseThrusters()
    {
        rb.AddForce(dir * force * Time.fixedDeltaTime);
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, isAttacking ? maxSpeed * 1.2f : maxSpeed);
        }
    }
    public void Move(Vector2 t)
    {
        target = t;
        hasOrder = true;
        isAttacking = false;
        followShip = null;
        line.SetPosition(1, t);
    }
    public void Attack(GameObject ship)
    {
        targetShip = ship;
        target = ship.transform.position;
        isAttacking = true;
        hasOrder = true;
        line.SetPosition(1, ship.transform.position);
    }
    public void Follow(GameObject ship)
    {
        followShip = ship;
        currentPosition = ship.transform.position - transform.position;
        if (currentPosition.magnitude > 5f)
        {
            currentPosition /= (currentPosition.magnitude / 5f);
        }
        hasOrder = true;
        line.SetPosition(1, ship.transform.position);
    }
    public void Select()
    {
        sprite.color = highlightColor;
        canvas.enabled = true;

    }
    public void UnSelect()
    {
        sprite.color = shipColor;
        canvas.enabled = false;
    }

}

