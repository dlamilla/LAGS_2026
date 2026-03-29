using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class Bag : MonoBehaviour
{

    public List<GameObject> fishList = new List<GameObject>();
    public float radius;

    public bool ThereAreFish => fishList.Count > 0;

    public int fishInList => fishList.Count;

    public static Bag instance;

    private void Awake()
    {
        instance = this;
    }

    public void RemoveFish()
    {

        foreach(GameObject fish in fishList)
        {
            fish.SetActive(false);
        }

        fishList.Clear();
    }

    public void AddFishToBag(GameObject fish)
    {
        fishList.Add(fish);
        fish.transform.SetParent(transform);

        Vector2 rnd = Random.insideUnitCircle * radius;

        fish.transform.localPosition = new Vector3(rnd.x, rnd.y, 0);

        fish.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
    }

    private void OnDrawGizmos()
    {


        //Handles.color = Color.red;

        //Handles.DrawWireDisc(transform.position, transform.forward, radius);
    }

}
