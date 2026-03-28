using UnityEngine;
using UnityEngine.UI;

public class OxigenBubbles_UI : MonoBehaviour
{
    [SerializeField] private Image oxygenBar;
    [SerializeField] private RectTransform barRect;
    [SerializeField] private RectTransform bubbles;

    void Update()
    {
        float t = oxygenBar.fillAmount;

        float minY = barRect.rect.yMin;
        float maxY = barRect.rect.yMax;

        float newY = Mathf.Lerp(minY, maxY, t);

        Vector2 pos = bubbles.anchoredPosition;
        pos.y = newY;
        bubbles.anchoredPosition = pos;
    }
}
