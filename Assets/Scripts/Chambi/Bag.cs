using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;

public class Bag : MonoBehaviour
{
    public float radius;

    public static Bag instance;

    private void Awake()
    {
        instance = this;
    }

    public void AddFishToBag(GameObject fish)
    {
        fish.transform.SetParent(transform);

        Vector2 rnd = Random.insideUnitCircle * radius;

        fish.transform.localPosition = new Vector3(rnd.x, rnd.y, 0);

        fish.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
    }

    private void OnDrawGizmos()
    {


        Handles.color = Color.red;

        Handles.DrawWireDisc(transform.position, transform.forward, radius);
    }

}
