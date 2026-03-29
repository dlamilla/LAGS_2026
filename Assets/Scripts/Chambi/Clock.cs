using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    public float maxTime;
    public float currentTime;
    public bool outOfTime;

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

        if(currentTime <= 0) outOfTime = true;
    }
}
