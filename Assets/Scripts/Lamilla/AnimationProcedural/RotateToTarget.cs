using UnityEngine;

public class RotateToTarget : MonoBehaviour
{
    public float rotationSpeed = 5f; // Speed of rotation
    private Vector2 direction;
    public float movementSpeed = 5f; // Speed of movement towards the target
    public Transform target; // Target to rotate towards
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        direction = target.position - transform.position; // Calculate direction to the target
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Calculate the angle to the target
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward); // Create a rotation based on the angle
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime); // Smoothly rotate towards the target

        Vector2 cursorPos = target.position; // Get the cursor position in world space
        transform.position = Vector2.MoveTowards(transform.position, cursorPos, movementSpeed * Time.deltaTime); // Move towards the cursor position
    }
}
