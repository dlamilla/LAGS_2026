using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class Shark : Fish
{
    [Header("Attack")]
    public float distanceToAttack;
    public bool isAttacking;
    private bool attackStarted;
    private Vector3 attackDir;
    private Vector3 lastPlayerPos;

    [Header("DetectWall")]
    public Vector3 offset;
    public Vector3 boxSize;
    public float boxAngle;
    public float boxDistance;
    public LayerMask wallLayer;

    [Header("Splines")]
    public SplineAnimate splineAttack;
    public SplineContainer splineContainer;
    public SplineAnimate splineEvade;

    [Space]
    public BoxCollider2D attackCollider;

    protected override void Update()
    {
        if(splineAttack != null)
        {
            if (splineAttack.enabled)
            {
                if (splineAttack.IsPlaying)
                {
                    isFacingRight = transform.right.x > 0;
                    DetectPlayer();

                    RaycastHit2D hit = Physics2D.BoxCast(transform.position + offset, boxSize, boxAngle ,transform.right, boxDistance, wallLayer);

                    if (hit)
                    {
                        Debug.Log("called");
                        splineAttack.Pause();
                        if (isFacingRight)
                        {
                            transform.rotation = Quaternion.Euler(0, 180, 0);
                        }
                        else
                        {
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                    }
                    return;
                }

                splineAttack.enabled = false;
            }
        }


        if (attackStarted)
        {
            LookAtTarget(lastPlayerPos);
            return;
        }

        base.Update();

        if (playerInSight)
        {
            LookAtTarget(player.transform.position);

            if (InRangeToAttack(distanceToAttack) && !isAttacking)
            {
                Attack();
            }
        }

    }

    private void LookAtTarget(Vector3 target)
    {
        Vector3 toPlayer = target - transform.position;
        float angle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private bool InRangeToAttack(float range)
    {
        Vector3 ToPlayer = player.transform.position - transform.position;

        float sqrMagnitude = ToPlayer.sqrMagnitude;

        float sqrRange = range * range;

        return sqrMagnitude <= sqrRange;
    }

    private void Attack()
    {
        StartCoroutine(AttackCor());
    }

    IEnumerator AttackCor()
    {
        isAttacking = true;

        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position - transform.right * 1.5f;

        float elapsedTime = 0;
        float duration = 1;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;

        }

        transform.position = endPos;

        lastPlayerPos = player.transform.position;
        attackDir = (lastPlayerPos - transform.position).normalized; 

        elapsedTime = 0;
        duration = .4f;
        startPos = transform.position;
        endPos = transform.position + attackDir * distanceToAttack;

        attackCollider.enabled = true;
        coll.isTrigger = true;


        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        splineAttack.Container = splineContainer;
        splineContainer.transform.SetParent(null);
        attackCollider.enabled = false;

        splineAttack.enabled = true;
        splineAttack.Play();

        yield return new WaitUntil(() => !splineAttack.IsPlaying);

        coll.isTrigger = false;
        splineContainer.transform.SetParent(transform);
        splineAttack.Container = null;
        splineAttack.NormalizedTime = 0;

        splineContainer.transform.localPosition = new Vector3(7.25f, .55f, 0);
        splineContainer.transform.rotation = transform.rotation;

        isAttacking = false;


        if (!playerInSight)
        {
            if (!isFacingRight)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent<Player>(out var player))
        {
            if (!player.gotHit)
            {
                player.RecieveHit(damage);
            }
        }
    }
}
