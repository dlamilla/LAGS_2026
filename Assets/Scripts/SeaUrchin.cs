using UnityEngine;

public class SeaUrchin : MonoBehaviour
{
    public float damage;
    public float distance; 
    public float speed; 

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * distance) * speed;

        transform.position = new Vector3(startPos.x, startPos.y + yOffset, startPos.z);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.TryGetComponent<Player>(out var player))
        {
            player.RecieveHit(damage);
            Destroy(gameObject);
        }
    }
}
