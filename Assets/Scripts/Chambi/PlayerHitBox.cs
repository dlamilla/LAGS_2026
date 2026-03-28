using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    private Player player;
    public Transform headPos;
    public int damage;
    public bool isAttacking;

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

        buffer = Physics2D.OverlapBoxAll(headPos.position, size, headPos.eulerAngles.z, fishMask);

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
                        fish.transform.SetParent(transform);
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

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;


        Matrix4x4 oldMatrix = Gizmos.matrix;

        Gizmos.matrix = Matrix4x4.TRS(
            headPos.position,
            Quaternion.Euler(0, 0, headPos.eulerAngles.z),
            Vector3.one
        );


        Gizmos.DrawWireCube(Vector3.zero, size);

        Gizmos.matrix = oldMatrix;
    }
}
