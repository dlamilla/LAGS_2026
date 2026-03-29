using UnityEngine;
using UnityEngine.Rendering;

public class Tentacle : MonoBehaviour
{
    public int length = 5; // Length of the tentacle
    public LineRenderer lineRenderer;
    public Vector3[] segmentsPoses;
    private Vector3[] segmentV;

    public Transform targetDir;
    public float targetDist;
    public float smoothSpeed;


    public float wiggleSpeed;
    public float wiggleMagnitude;
    public Transform wiggleDir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer.positionCount = length; // Set the number of positions in the LineRenderer to match the length of the tentacle
        segmentsPoses = new Vector3[length]; // Initialize the array to hold the positions of each segment of the tentacle
        segmentV = new Vector3[length];
    }

    // Update is called once per frame
    void Update()
    {
        wiggleDir.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude); // Apply a wiggling rotation to the wiggleDir based on a sine wave

        segmentsPoses[0] = targetDir.position; // Set the position of the first segment to the target's position
        for(int i = 1; i < segmentsPoses.Length; i++)
        {
            Vector3 targetPos = segmentsPoses[i - 1] + (segmentsPoses[i] - segmentsPoses[i - 1]).normalized * targetDist; // Calculate the target position for the current segment based on the previous segment's position and the desired distance
            segmentsPoses[i] = Vector3.SmoothDamp(segmentsPoses[i], targetPos, ref segmentV[i], smoothSpeed); // Smoothly move the current segment towards the target position using SmoothDamp
        }
        lineRenderer.SetPositions(segmentsPoses); // Update the LineRenderer with the new positions of the segments
    }
}
