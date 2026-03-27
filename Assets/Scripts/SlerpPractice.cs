using UnityEngine;

public class SlerpPractice : MonoBehaviour
{
    public Vector3 pointA;
    public Vector3 pointB;
    public Transform controlPoint;
    public float duration = 2f;

    private float time;

    private void Start()
    {
        pointA = transform.position;
        pointB = transform.position + transform.right * 4;
    }

    void Update()
    {
        time += Time.deltaTime;
        float t = time / duration;

        Vector3 pos =
            Mathf.Pow(1 - t, 2) * pointA +
            2 * (1 - t) * t * controlPoint.position +
            Mathf.Pow(t, 2) * pointB;

        transform.position = pos;
    }
}
