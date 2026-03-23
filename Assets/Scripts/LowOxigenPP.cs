using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LowOxigenPP : MonoBehaviour
{
    [SerializeField] private Player player;
    [Header("Settings")]
    public float minIntensity;
    public float maxIntensity;
    public float firstDuration;
    public float duration;

    private bool firstIntensity;

    private Volume volume;
    private Vignette vignette;

    private void Awake()
    {
        volume = GetComponent<Volume>();
    }

    void Start()
    {
        volume.profile.TryGet(out vignette);
        firstIntensity = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine(Cor());
        }
    }

    IEnumerator Cor()
    {
        while (true)
        {
            float elapsedTime = 0;

            if (firstIntensity)
            {
                while (elapsedTime < firstDuration)
                {
                    vignette.intensity.value = Mathf.Lerp(0, maxIntensity, elapsedTime / firstDuration);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                vignette.intensity.value = maxIntensity;
            }
            else
            {
                while (elapsedTime < duration)
                {
                    vignette.intensity.value = Mathf.Lerp(minIntensity, maxIntensity, elapsedTime / duration);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                vignette.intensity.value = maxIntensity;
            }


            yield return new WaitForSeconds(.4f);

            firstIntensity = false;

            elapsedTime = 0;

            while (elapsedTime < duration)
            {
                vignette.intensity.value = Mathf.Lerp(maxIntensity, minIntensity, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            vignette.intensity.value = minIntensity;

            yield return new WaitForSeconds(.4f);
        }
    }
}
