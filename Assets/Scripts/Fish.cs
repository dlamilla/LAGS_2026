using UnityEngine;

public class Fish : MonoBehaviour
{
    [Header("Data")]
    public FishData data;
    [Header("Captured Info")]
    public float frequency;
    public float distance;

    public bool isCaptured;

    private Vector3 startPos;

    private CapsuleCollider2D capsuleCollider;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
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

        transform.position += transform.right * data.moveSpeed * Time.deltaTime;
    }

    public void FishCaptured()
    {
        isCaptured = true;
        capsuleCollider.isTrigger = true;
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
}
