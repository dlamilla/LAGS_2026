using TMPro;
using UnityEngine;

public class WeightCounter : MonoBehaviour
{
    public Player player;

    private TextMeshProUGUI counterText;

    private void Awake()
    {
        counterText = GetComponent<TextMeshProUGUI>();
    }


    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        counterText.text = $"{player.currentWeight}/{player.maxWeightToLift}";
    }

}
