using UnityEngine;

public class Boat : MonoBehaviour
{
    public float waveHeight = 0.3f;
    public float waveSpeed = 1f;
    public float waveFrequency = 1f;

    float baseY;

    void Start()
    {
        baseY = transform.position.y;
    }

    void Update()
    {
        float x = transform.position.x;

        float wave =
        Mathf.Sin(x * waveFrequency + Time.time * waveSpeed) * waveHeight;

        transform.position = new Vector3(
            x,
            baseY + wave,
            transform.position.z
        );
    }
}
