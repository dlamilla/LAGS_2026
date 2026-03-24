using UnityEngine;

public class Fish : MonoBehaviour
{
    public Transform player;

    [Header("General")]
    public float moveSpeed;
    [Space]
    public int health;
    public int damage;
    public int score;
    public int weight;

    [Header("Detection")]
    public float visionAngle;
    public float visionRange;
    public Vector3 offset;
    public bool playerInSight;
    [Space]
    public float secondVisionAngle;
    public float secondVisionRange;

    public bool isCaptured;

    private Vector3 startPos;

    private Collider2D coll;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCaptured) return;

        transform.position += transform.right * moveSpeed * Time.deltaTime;

        DetectPlayer();
    }

    public void RecieveHit(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            FishCaptured();
        }
    }

    public void FishCaptured()
    {
        isCaptured = true;
        coll.isTrigger = true;
        coll.enabled = false;
    }

    private void DetectPlayer()
    {
        Vector3 origin = transform.position + transform.TransformDirection(offset);
        Vector2 toPlayer = player.position - origin;
        float sqrMagnitude = toPlayer.sqrMagnitude;
        float sqrRange = visionRange * visionRange;

        float angle = Vector2.Angle(transform.right, toPlayer.normalized);

        if (sqrMagnitude <= sqrRange && angle <= visionAngle)
        {
            playerInSight = true;
            Debug.Log(angle);
            return;
        }
        else
        {
            playerInSight = false;
        }

        float sqrSecondRange = secondVisionRange * secondVisionRange;

        float secondAngle = Vector2.Angle(transform.right, toPlayer.normalized);

        if (sqrMagnitude <= sqrSecondRange && secondAngle <= secondVisionAngle)
        {
            playerInSight = true;

            Debug.Log(angle);
        }
        else
        {
            playerInSight = false;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if(transform.rotation.y == 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Vector3 worldOffset = transform.TransformDirection(offset);

        Vector3 rightAngle = Quaternion.Euler(0, 0, visionAngle) * transform.right;

        Gizmos.DrawLine(transform.position + worldOffset, transform.position + worldOffset + rightAngle * visionRange);

        Vector3 leftAngle = Quaternion.Euler(0, 0, -visionAngle) * transform.right;

        Gizmos.DrawLine(transform.position + worldOffset, transform.position + worldOffset + leftAngle * visionRange);

        Gizmos.color = Color.red;

        Vector3 secondRightAngle = Quaternion.Euler(0, 0, secondVisionAngle) * transform.right;

        Gizmos.DrawLine(transform.position + worldOffset, transform.position + worldOffset + secondRightAngle * secondVisionRange);

        Vector3 secondLeftAngle = Quaternion.Euler(0, 0, -secondVisionAngle) * transform.right;

        Gizmos.DrawLine(transform.position + worldOffset, transform.position + worldOffset + secondLeftAngle * secondVisionRange);

        Vector2 toPlayer = player.position - transform.position;
    }
}
