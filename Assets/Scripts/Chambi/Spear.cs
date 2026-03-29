using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class Spear : MonoBehaviour
{
    private Player player;
    public int damage;
    public bool isAttacking;
    public Transform point;

    [Header("Collision Info")]
    public Vector3 size;
    public Vector3 offset;
    public LayerMask fishMask;
    public Collider2D[] buffer;
    private readonly HashSet<Collider2D> fishBuffer = new();

    public bool IsAttackOnCooldown { get; private set; }

    private CinemachineImpulseSource source;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        source = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        if (!isAttacking) return;

        Vector2 rotatedOffset = transform.rotation * offset;

        buffer = Physics2D.OverlapBoxAll((Vector2)transform.position + rotatedOffset, size, transform.eulerAngles.z, fishMask);

        foreach (var fish in buffer)
        {
            if (!fishBuffer.Contains(fish))
            {
                fishBuffer.Add(fish);

                if (fish.transform.TryGetComponent<Fish>(out var f))
                {
                    f.RecieveHit(damage);

                    if (f.isCaptured)
                    {
                        player.AddWeight(f.weight);
                        fish.transform.SetParent(point);
                        fish.transform.position = point.position;
                    }
                }

                source.GenerateImpulse();
            }
        }
    }

    public void ActivateHitBox()
    {
        fishBuffer.Clear();
        IsAttackOnCooldown = true;
        isAttacking = true;
    }

    public void DeactivateHitBox()
    {
        isAttacking = false;
        IsAttackOnCooldown = false;
    }


    //private void DetectCollision()
    //{
    //    Vector3 worldOffset = transform.TransformDirection(offset);

    //    buffer = Physics2D.OverlapBoxAll(transform.position + worldOffset, size, transform.eulerAngles.z, fishMask);

    //    foreach (var fish in buffer)
    //    {
    //        if (!fishBuffer.Contains(fish))
    //        {
    //            fishBuffer.Add(fish);

    //            if (fish.transform.TryGetComponent<Fish>(out var f))
    //            {
    //                f.RecieveHit(damage);

    //                if (f.isCaptured)
    //                {
    //                    player.AddWeight(f.weight);
    //                    fish.transform.SetParent(transform);
    //                }
    //            }

    //            source.GenerateImpulse();
    //        }
    //    }
    //}

    //IEnumerator ThrowSpear()
    //{
    //    IsAttackOnCooldown = true;

    //    float duration = 0.1f;
    //    float elapsedTime = 0;

    //    Vector3 startPos = transform.localPosition;
    //    Vector3 endPos = startPos + Vector3.right * distance;

    //    while (elapsedTime < duration)
    //    {
    //        float t = elapsedTime / duration;

    //        transform.localPosition = Vector3.Lerp(startPos, endPos, t);

    //        DetectCollision();

    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    transform.localPosition = endPos;

    //    yield return new WaitForSeconds(.5f);

    //    elapsedTime = 0;
    //    duration = .3f;
        
    //    while (elapsedTime < duration)
    //    {
    //        float t = elapsedTime / duration;

    //        transform.localPosition = Vector3.Lerp(endPos, startPos, t);

    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    transform.localPosition = startPos;

    //    IsAttackOnCooldown = false;
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = isAttacking ? Color.red : Color.yellow;

        float angle = transform.eulerAngles.z;

        Vector2 rotatedOffset = transform.rotation * offset;

        Matrix4x4 oldMatrix = Gizmos.matrix;

        Gizmos.matrix = Matrix4x4.TRS(
            transform.position + (Vector3)rotatedOffset,
            Quaternion.Euler(0, 0, angle),
            Vector3.one
        );

        Gizmos.DrawWireCube(Vector3.zero, size);

        Gizmos.matrix = oldMatrix;
    }
}
