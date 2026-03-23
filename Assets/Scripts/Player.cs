using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Locomotion")]
    public float maxMoveSpeed;
    public float currentMoveSpeed;
    public float sinkSpeed;
    public float originalSinkSpeed;

    [Header("Oxigen")]
    public float maxOxigen;
    public float currentOxigen;
    public float consumingOxigenSpeed;

    [Header("Weights")]
    public float currentWeight;
    public float maxWeightToLift;

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

        if (Input.GetKeyDown(KeyCode.T))
        {
            AddWeight(1);
        }

    }

    private void FixedUpdate()
    {
        Vector3 nextPos = transform.position + moveDir * currentMoveSpeed * Time.fixedDeltaTime;

        if (moveDir == Vector3.zero || currentMoveSpeed == 0)
        {
            if(currentWeight == 0)
            {
                nextPos.y -= originalSinkSpeed * Time.fixedDeltaTime;
            }
            else
            {
                nextPos.y -= sinkSpeed * Time.fixedDeltaTime;
            }
        }

        rb.MovePosition(nextPos);
        rb.MoveRotation(angle);
    }

    private void ConsumingOxigen()
    {
        currentOxigen -= consumingOxigenSpeed * Time.deltaTime;

        oxigenBar.fillAmount = currentOxigen/maxOxigen;
    }

    public void AddWeight(float weight)
    {

        if (currentMoveSpeed > 0)
        {
            currentMoveSpeed -= weight;
        }

        if (currentWeight < maxWeightToLift)
        {
            currentWeight += weight;
            sinkSpeed += weight;
        }
    }

    public void ReduceWeight(float weight)
    {
        sinkSpeed -= weight;
    }
}
