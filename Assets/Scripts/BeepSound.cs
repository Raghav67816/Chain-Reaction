using System;
using UnityEngine;

public class BeepSound : MonoBehaviour
{
    public static BeepSound Instance {get; private set; }
    
    public AudioSource audioSource;
    private AudioClip audioClip;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource.playOnAwake = false;
    }

    public void PlayAudio()
    {
        GenerateBeep();
        if(audioSource != null && audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }

    private AudioClip GenerateBeep(int samplingRate = 44100, float duration = 0.2f)
    {
        int sampleCount = (int)(samplingRate*duration);
        float[] samples = new float[sampleCount];


        for(int i=0; i < sampleCount; i++)
        {
            float t = (float)i / samplingRate;
            float wave = Mathf.Sin(2f * Mathf.PI * 900f * t);

            float squareWave = wave >= 0 ? 1f : -1f;

            float envolope = Mathf.Exp(-i/(sampleCount * 0.3f));
            samples[i] = squareWave * envolope * 0.25f;
        }

        audioClip = AudioClip.Create("Quick Beep", sampleCount, 1, samplingRate, false);
        audioClip.SetData(samples, 0);
        return audioClip;
    }
}
