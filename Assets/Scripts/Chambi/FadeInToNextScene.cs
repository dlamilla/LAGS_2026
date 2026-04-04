using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInToNextScene : MonoBehaviour
{
    public Player player;
    public FishCounter fishCounter;
    public Clock clock;
    private CanvasGroup canvasGroup;

    bool called;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fishCounter.currentFish >= fishCounter.maxFish && !called)
        {
            StartCoroutine(Cor("Ganar"));
        }

        if((player.isDead || clock.outOfTime) && !called)
        {
            StartCoroutine (Cor("Perder"));
        }
    }

    IEnumerator Cor(string sceneName)
    {
        called = true;
        float elapsedTime = 0;
        float duration = 3;

        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;

        yield return new WaitForSeconds(.5f);

        SceneManager.LoadScene(sceneName);
    }
}
