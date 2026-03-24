using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    private Player player;
    public float distance;

    [Header("Collision Info")]
    public Vector3 size;
    public Vector3 offset;
    public LayerMask fishMask;
    public Collider2D[] buffer;
    private readonly HashSet<Collider2D> fishBuffer = new();

    public bool IsAttackOnCooldown { get; private set; }

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void ActivateSpearAttack()
    {
        StartCoroutine(ThrowSpear());
    }

    IEnumerator ThrowSpear()
    {
        IsAttackOnCooldown = true;

        float duration = 0.1f;
        float elapsedTime = 0;

        Vector3 startPos = transform.localPosition;
        Vector3 endPos = startPos + Vector3.right * distance;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            transform.localPosition = Vector3.Lerp(startPos, endPos, t);

            Vector3 worldOffset = transform.TransformDirection(offset);

            buffer = Physics2D.OverlapBoxAll(transform.position + worldOffset, size, transform.eulerAngles.z, fishMask);

            foreach (var fish in buffer)
            {
                if (!fishBuffer.Contains(fish))
                {
                    if(fish.transform.TryGetComponent<Fish>(out var f))
                    {
                        f.FishCaptured();
                    }

                    fishBuffer.Add(fish);
                    fish.transform.SetParent(transform);
                    player.AddWeight(f.data.weight);
                }
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = endPos;

        yield return new WaitForSeconds(.5f);

        elapsedTime = 0;
        duration = .3f;
        
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            transform.localPosition = Vector3.Lerp(endPos, startPos, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = startPos;

        IsAttackOnCooldown = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, Quaternion.Euler(0, 0, transform.eulerAngles.z), Vector3.one);

        Gizmos.matrix = rotationMatrix;

        Gizmos.DrawWireCube(offset, size);

        Gizmos.matrix = Matrix4x4.identity;
    }
}
