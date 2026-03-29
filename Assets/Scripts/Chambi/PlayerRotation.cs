using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    private Camera main;

    private Player player;

    private bool called;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    void Start()
    {
        main = Camera.main;
    }

    void Update()
    {
        if(player.isDead) return;

        if (player.hasWon)
        {
            if (called) return;
            Vector3 mousePos = main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = transform.position.z;
            Vector3 dir = mousePos - transform.position;

            float dot = Vector2.Dot(Vector2.right, dir.normalized);

            if (dot < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);

            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            called = true;

            return;
        }

        if(player.MoveDir == Vector3.zero)
        {
            if (player.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                RotatePlayerToMousePosition();
                return;
            }

            Vector3 mousePos = main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = transform.position.z;
            Vector3 dir = mousePos - transform.position;

            float dot = Vector2.Dot(Vector2.right, dir.normalized);

            if(dot < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);

            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            
            return;
        }

        RotatePlayerToMousePosition();
    }

    private void RotatePlayerToMousePosition()
    {
        Vector3 mousePos = main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;
        Vector3 dir = mousePos - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        float dot = Vector2.Dot(Vector2.right, dir.normalized);

        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (dot < 0)
        {
            transform.rotation = Quaternion.Euler(-180, 0, -transform.eulerAngles.z);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, transform.eulerAngles.z);
        }
    }
}
