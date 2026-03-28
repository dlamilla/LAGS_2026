using System.Collections;
using UnityEngine;

public class OxygenBubble : MonoBehaviour
{
    public Vector2 lifespanValues;
    public float lifespan;
    public float oxygenGiven;

    public float elevateSpeed;

    private void OnEnable()
    {
        float l = Random.Range(lifespanValues.x, lifespanValues.y);

        lifespan = Mathf.Round(l);
    }

    private void Update()
    {
        transform.position += transform.up * elevateSpeed * Time.deltaTime;

        lifespan -= Time.deltaTime;

        if(lifespan <= 0)
        {
            DisableBubble();
        }
    }

    public void DisableBubble()
    {
        gameObject.SetActive(false);
        BubblesGenerator.instance.ReturnToQueue(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent<Player>(out var player))
        {
            player.AddOxigen(oxygenGiven);
            DisableBubble();
        }
    }
}
