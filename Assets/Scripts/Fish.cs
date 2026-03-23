using UnityEngine;

public class Fish : MonoBehaviour
{
    [Header("Data")]
    public FishData data;
    [Header("Captured Info")]
    public float frequency;
    public float distance;

    public bool isCaptured;

    private Vector3 startPos;

    private CircleCollider2D circleCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //if (isCaptured) return;

        //float yOffset = Mathf.Sin(Time.time * frequency) * distance;
        //transform.position = startPos + new Vector3(0, yOffset, 0);
    }
}
