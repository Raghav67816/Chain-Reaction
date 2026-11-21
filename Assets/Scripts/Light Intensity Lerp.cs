using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightIntensityLerp : MonoBehaviour
{
    [Header("Animation Settings")]
    public float lerpSpeed;
    public float interval;
    public float minIntensity;
    public float maxIntensity;

    [Header("Interval")]
    public float minInterval;
    public float maxInterval;

    [Header("Extras")]
    [Range(0f, 1f)] public float chance = 0.2f;
    public float spikeIntensity = 2.5f; // has to decrease enormously 

    [Header("Light Object")]
    public Light2D lightObject;

    private float intensity;

    void Start()
    {
        intensity = lightObject.intensity;

        StartCoroutine(GenerateRandomIntensity());
    }

    void Update()
    {
        lightObject.intensity = Mathf.Lerp(
            lightObject.intensity,
            intensity,
            lerpSpeed * Time.deltaTime
        );
    }


    private IEnumerator GenerateRandomIntensity()
    {
        while (true)
        {

            if(Random.value < chance)
            {
                intensity = spikeIntensity;
            }

            else
            {
                intensity = Random.Range(minIntensity, maxIntensity);
            }

            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
