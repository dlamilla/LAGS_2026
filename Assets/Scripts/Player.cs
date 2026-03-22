using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Locomotion")]
    public float moveSpeed;
    public float sinkSpeed;

    [Header("Oxigen")]
    public float maxOxigen;
    public float currentOxigen;
    public float consumingOxigenSpeed;

    [Header("UI")]
    public Image oxigenBar;

    #region NotShowedInInspector

    private Rigidbody2D rb;

    private float xInput, yInput;

    private Vector3 moveDir;

    private Camera main;

    private float angle;

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        main = Camera.main;
    }

    private void Start()
    {
        currentOxigen = maxOxigen;
    }

    void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        moveDir = new Vector3(xInput, yInput, 0).normalized;

        Vector3 mousePos = main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;

        Vector2 dir = mousePos - transform.position;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        ConsumingOxigen();
    }

    private void FixedUpdate()
    {
        Vector3 nextPos = transform.position + moveDir * moveSpeed * Time.fixedDeltaTime;

        if (moveDir == Vector3.zero)
        {
            nextPos.y -= sinkSpeed * Time.fixedDeltaTime;
        }

        rb.MovePosition(nextPos);
        rb.MoveRotation(angle);
    }

    private void ConsumingOxigen()
    {
        currentOxigen -= consumingOxigenSpeed * Time.deltaTime;

        oxigenBar.fillAmount = currentOxigen/maxOxigen;
    }
}
