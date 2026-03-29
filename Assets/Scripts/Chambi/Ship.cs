using UnityEngine;

public class Ship : MonoBehaviour
{
    public GameObject recibir;

    public bool canReceiveFish;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            recibir.SetActive(true);
            canReceiveFish= true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            recibir.SetActive(false);
            canReceiveFish= false;
        }
    }
}
