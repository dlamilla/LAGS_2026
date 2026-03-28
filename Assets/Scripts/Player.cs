using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public bool useMouse;
    [Header("Locomotion")]
    public float maxMoveSpeed;
    public float currentMoveSpeed;
    public float sinkSpeed;
    public float originalSinkSpeed;

    [Header("Dash")]
    public float dashSpeed;
    public float dashDuration;
    public float dashCooldown;
    private bool isDashInCooldown;
    private bool isDashing;

    private Vector2 dashDir;

    [Header("General")]
    public bool gotHit;

    [Header("Oxigen")]
    public float maxOxigen;
    public float currentOxigen;
    public float consumingOxigenSpeed;
    public float dashOxigenConsumption;

    [Header("Weights")]
    public float currentWeight;
    public float maxWeightToLift;

    [Header("Spear")]
    public Spear spear;

    [Header("UI Related")]
    public LowOxigenPP lowOxigen;
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

        if (useMouse)
        {
            
        }
        

        ConsumingOxigen();

        if (Input.GetMouseButtonDown(0) && !spear.IsAttackOnCooldown)
        {
            spear.ActivateSpearAttack();
            Vector3 mousePos = main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = transform.position.z;

            Vector2 dir = mousePos - transform.position;
            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }


        if (Input.GetKeyDown(KeyCode.Space) && !isDashInCooldown)
        {
            StartCoroutine(Dash());
        }

        if(currentOxigen <= maxOxigen * 30/100 && !lowOxigen.fX_Active)
        {
            StartCoroutine(lowOxigen.VignetteFX());
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            rb.linearVelocity = dashDir * dashSpeed;
            return;
        }

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

    IEnumerator Dash()
    {
        isDashInCooldown = true;
        isDashing = true;

        if(moveDir == Vector3.zero)
        {
            dashDir = transform.right;
        }
        else
        {
            dashDir = moveDir;
        }

        ConsumeOxigen(dashOxigenConsumption);

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(dashCooldown);

        isDashInCooldown = false;
    }

    public void AddOxigen(float amount)
    {
        currentOxigen += amount;
        if(currentOxigen > maxOxigen)
        {
            currentOxigen = maxOxigen;
        }
    }

    private void ConsumingOxigen()
    {
        currentOxigen -= consumingOxigenSpeed * Time.deltaTime;

        oxigenBar.fillAmount = currentOxigen/maxOxigen;
    }

    public void ConsumeOxigen(float value)
    {
        currentOxigen -= value;
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

    public void RecieveHit(float damage)
    {
        ConsumeOxigen(damage);
        if (!gotHit) StartCoroutine(ResetCor());
    }

    IEnumerator ResetCor()
    {
        gotHit = true;
        yield return new WaitForSeconds(2);

        gotHit = false;
    }
}
