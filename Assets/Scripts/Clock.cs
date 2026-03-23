using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    public float maxTime;
    public float currentTime;

    [Header("UI")]
    public Image clockImg;

    void Start()
    {
        currentTime = maxTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;

        clockImg.fillAmount = currentTime / maxTime;
    }
}
