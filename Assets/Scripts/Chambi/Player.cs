using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Ship ship;
    public Clock clock;

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
    public bool isDead;
    public bool hasWon;
    public bool hasLost;

    [Header("Oxigen")]
    public float maxOxigen;
    public float currentOxigen;
    public float consumingOxigenSpeed;
    public float dashOxigenConsumption;

    [Header("Weights")]
    public float currentWeight;
    public float maxWeightToLift;

    [Header("HitBox")]
    public PlayerHitBox hitBox;
    public Spear spear;

    [Header("UI Related")]
    public LowOxigenPP lowOxigen;
    public Image oxigenBar;

    #region NotShowedInInspector

    private Rigidbody2D rb;

    private float xInput, yInput;

    public Vector3 MoveDir { get; private set; }

    public Animator anim;

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        currentOxigen = maxOxigen;
    }

    void Update()
    {
        if (isDead)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
            {
                anim.Play("Death");
            }

            MoveDir = Vector3.zero;
            return;
        }

        if (hasWon || clock.outOfTime)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                anim.Play("Idle");
            }

            MoveDir = Vector3.zero;
            return;
        }

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        MoveDir = new Vector3(xInput, yInput, 0).normalized;

        Animations();

        ConsumingOxigen();

        if(Input.GetKeyDown(KeyCode.E) && ship.canReceiveFish)
        {
            StoreFish();
        }

        if(currentOxigen <= 0)
        {
            isDead = true;
        }

        if (Input.GetMouseButtonDown(0) && !spear.IsAttackOnCooldown)
        {
            anim.Play("Attack");
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

        if (hasWon) return;
        if (isDashing)
        {
            rb.linearVelocity = dashDir * dashSpeed;
            return;
        }

        Vector3 nextPos = transform.position + currentMoveSpeed * Time.fixedDeltaTime * MoveDir;

        if (MoveDir == Vector3.zero || currentMoveSpeed == 0)
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
    }

    IEnumerator Dash()
    {
        isDashInCooldown = true;
        isDashing = true;

        if(MoveDir == Vector3.zero)
        {
            dashDir = transform.right;
        }
        else
        {
            dashDir = MoveDir;
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

    public void StoreFish()
    {
        currentMoveSpeed = maxMoveSpeed;
        sinkSpeed = originalSinkSpeed;
        Bag.instance.RemoveFish();
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

    private void Animations()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                if (MoveDir == Vector3.zero)
                {
                    anim.Play("Idle");
                }
                else
                {
                    anim.Play("Swim");
                }
            }
            return;
        }

        if (MoveDir == Vector3.zero && !anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            anim.Play("Idle");
        }

        if (MoveDir != Vector3.zero && !anim.GetCurrentAnimatorStateInfo(0).IsName("Swim"))
        {
            anim.Play("Swim");
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Surface"))
        {
            currentOxigen = maxOxigen;
        }
    }
}
