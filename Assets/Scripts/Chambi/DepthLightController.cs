using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Splines.Interpolators;

public class DepthLightController : MonoBehaviour
{
    public Transform player;
    public Transform surface;
    public Transform seabed;

    public float maxIntensity;
    public float minIntensity;

    [Range(0f, 1f)]
    public float startDarkeningPercent;

    [Range(0f, 1f)]
    public float fullDarknessPercent;

    private Light2D globalLight;

    private void Awake()
    {
        globalLight = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float normalizedDepth = Mathf.InverseLerp(surface.position.y, seabed.position.y, player.position.y);

        float t = Mathf.InverseLerp(startDarkeningPercent, fullDarknessPercent, normalizedDepth);

        globalLight.intensity = Mathf.Lerp(maxIntensity, minIntensity, t);

    }
}
