using TMPro;
using UnityEngine;

public class FishCounter : MonoBehaviour
{
    public Player player;
    public float maxFish; 
    public float currentFish;

    private TextMeshProUGUI counterText;

    private void Awake()
    {
        counterText = GetComponent<TextMeshProUGUI>();
    }

    //private void OnEnable()
    //{
    //    EventBus.OnFishCapturedEvent += AddFish;
    //}

    //private void OnDisable()
    //{
    //    EventBus.OnFishCapturedEvent -= AddFish;
    //}

    private void Start()
    {
        currentFish = 0;
        UpdateUI();
    }

    //public void AddFish()
    //{
    //    currentFish++;

    //    if (currentFish >= maxFish)
    //    {
    //        currentFish = maxFish;
    //        player.hasWon = true;
    //    }

    //    UpdateUI();
    //}

    public void UpdateUI()
    {
        counterText.text = $"{currentFish}/{maxFish}";
    }

}
