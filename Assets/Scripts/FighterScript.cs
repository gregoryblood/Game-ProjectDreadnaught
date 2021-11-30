using UnityEngine;

public class FighterScript : MonoBehaviour
{

    [SerializeField] float turnSpeed = 10f;
    [SerializeField] float speed = 5;
    [SerializeField] AudioClip warpSound;
    public int teamNumber;
    public Vector3 moveTarget;
    public GameObject attackTarget;
    public ObjectiveScript [] planets = new ObjectiveScript[10];
    int currentObjNum = 0;
    Rigidbody2D rb;
    int frameDif;
    Transform playerCapital;
    SpriteRenderer sprite;
    bool warping;
    AudioSource audioSource;
    Transform hull;
    // Start is called before the first frame update
    void Start()
    {
        ColorSetter cl = ColorSetter.Instance;
        Color color = cl.GetColor(teamNumber);
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        GetComponent<LineRenderer>().startColor = color;
        GetComponent<LineRenderer>().endColor = color;
        sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.color = color;
        ShipDefence sd = GetComponent<ShipDefence>();
        sd.teamNumber = teamNumber;
        sd.shipColor = color;
        GetComponent<LaserBeam>().teamNumber = teamNumber;
        frameDif = Random.Range(0, 20);
        currentObjNum = 0;
        hull = GetComponentInChildren<CircleCollider2D>().transform;
        float dist = Mathf.Infinity;
        for (int i = 0; i < planets.Length; i++)
        {
            if (!planets[i])
                break;
            float d = Vector3.Distance(transform.position, planets[i].transform.position);
            if (d < dist)
            {
                dist = d;
                currentObjNum = i;
            }
        }
        MoveToNextObjective(planets[currentObjNum].transform.position);
        if (teamNumber != 0)
        {
            GameObject playerCap;
            if (playerCap = GameObject.Find("PlayerCapital"))
                playerCapital = playerCap.transform;
        }
        turnSpeed -= Random.Range(0f, 0.6f);
        hull.transform.localPosition -= (Vector3.up * 200f);
        warping = true;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (warping) return;
        //Point to target
        Vector2 dir = (attackTarget ? attackTarget.transform.position : moveTarget) - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, q, turnSpeed * Time.deltaTime);

        rb.MovePosition(rb.position + ((Vector2)transform.up * speed * Time.fixedDeltaTime));

    }
    private void Update()
    {
        if (warping)
        {
            UseWarper();
            return;
        }
        if ((frameDif + Time.frameCount) % 10 != 0) return;
        if (!attackTarget)
        {
            //Objective movement
            if (!planets[currentObjNum])
            {
                if (playerCapital)
                    MoveToNextObjective(playerCapital.position);
                return;
            }
            else if (Vector3.Distance(transform.position, moveTarget) < 2f)
            {
                if (planets[currentObjNum].holdingTeam == teamNumber)
                {
                    currentObjNum++;
                    if (planets[currentObjNum])
                        MoveToNextObjective(planets[currentObjNum].transform.position);
                }
            }
        }
    }
    void UseWarper()
    {
        hull.transform.localPosition += (Vector3.up * 200f * Time.deltaTime);

        if (hull.transform.localPosition.y > 0)
        {
            hull.transform.localPosition = Vector3.zero;
            warping = false;
            float oldpitch = audioSource.pitch;
            float oldvolume = audioSource.volume;
            audioSource.pitch = 3f;
            audioSource.volume = 0.1f;
            audioSource.PlayOneShot(warpSound);
            audioSource.pitch = oldpitch;
            audioSource.volume = oldvolume;
        }
    }
    public void MoveToNextObjective(Vector2 position)
    {
        moveTarget = position + (Random.insideUnitCircle * (planets[currentObjNum] ? planets[currentObjNum].radius : 10f));
    }
}
