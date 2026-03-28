using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeINOUT : MonoBehaviour
{
    public static FadeINOUT instance;

    public Image fadeImage;
    public float fadeTime = 1f;

    bool isFading = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float t = 0;
        Color color = fadeImage.color;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, t / fadeTime);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 0;
        fadeImage.color = color;
    }

    IEnumerator FadeOut()
    {
        float t = 0;
        Color color = fadeImage.color;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, t / fadeTime);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1;
        fadeImage.color = color;
    }

    public void LoadScene(int sceneName)
    {
        if (!isFading)
            StartCoroutine(FadeAndLoad(sceneName));
    }

    IEnumerator FadeAndLoad(int sceneName)
    {
        isFading = true;

        yield return StartCoroutine(FadeOut());

        SceneManager.LoadScene(sceneName);

        yield return null;

        yield return StartCoroutine(FadeIn());

        isFading = false;
    }
}