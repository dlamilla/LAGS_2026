using TMPro;
using UnityEngine;

public class FishCounter : MonoBehaviour
{
    public Player player;
    public int maxFish; 
    public int currentFish = 0;

    private TextMeshProUGUI counterText;

    private void Awake()
    {
        counterText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        EventBus.OnFishCapturedEvent += AddFish;
    }

    private void OnDisable()
    {
        EventBus.OnFishCapturedEvent -= AddFish;
    }

    private void Start()
    {
        UpdateUI();
    }

    private void AddFish()
    {
        currentFish++;

        if (currentFish >= maxFish)
        {
            currentFish = maxFish;
            player.hasWon = true;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        counterText.text = $"{currentFish}/{maxFish}";
    }

}
