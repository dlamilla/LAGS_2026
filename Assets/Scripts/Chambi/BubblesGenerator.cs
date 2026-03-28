using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblesGenerator : MonoBehaviour
{
    public float generateBubblesInterval;

    public GameObject bubblePrefab;

    public List<Transform> bubblePoints = new();

    public Queue<GameObject> oxygenBubbleQueue = new();

    public int lastRnd = -1;

    public static BubblesGenerator instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        foreach (Transform t in transform)
        {
            bubblePoints.Add(t);
        }

        for (int i = 0; i < 4; i++)
        {
            AddNewBubble();
        }

        StartCoroutine(GenerateBubbles());
    }

    IEnumerator GenerateBubbles()
    {
        while (true)
        {
            int activateRnd;

            do
            {
                activateRnd = Random.Range(0, bubblePoints.Count);
            }
            while (activateRnd == lastRnd); //esto será un loop infinito si solo hay un bubblePoint, 
                                            //la solucion solo sería que bubblePoints > 1 pero así se ve mas piola

            lastRnd = activateRnd;

            if (oxygenBubbleQueue.Count == 0)
            {
                AddNewBubble();
            }

            GameObject bubble = oxygenBubbleQueue.Dequeue();

            bubble.transform.position = bubblePoints[activateRnd].position;

            bubble.SetActive(true);

            yield return new WaitForSeconds(generateBubblesInterval);

        }
    }

    private void AddNewBubble()
    {
        GameObject bubble = Instantiate(bubblePrefab);
        bubble.transform.SetParent(transform);
        oxygenBubbleQueue.Enqueue(bubble);
        bubble.SetActive(false);
    }

    public void ReturnToQueue(GameObject bubble)
    {
        oxygenBubbleQueue.Enqueue(bubble);
    }
}
