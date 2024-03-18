using System.Collections;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public float maxVolume = 1f;
    public float minVolume = 0f;
    private AudioSource source;
    public AudioClip clip;

    public float fadeInDuration = 2f;
    public float fadeOutDuration = 2f;

    private IEnumerator fadeIn;
    private IEnumerator fadeOut;

    public static AudioController instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        source = GetComponent<AudioSource>();
    }

    public void StartMusic()
    {
        if (fadeOut != null)
        {
            StopCoroutine(fadeOut);
        }

        source.clip = clip;
        source.Play();
        
        fadeIn = FadeIn(source, fadeInDuration, maxVolume);
        
        StartCoroutine(fadeIn);
    }

    public void StopMusic()
    {
        fadeOut = FadeOut(source, fadeOutDuration, minVolume);
        if (source.isPlaying)
        {
            StopCoroutine(fadeIn);
            StartCoroutine(fadeOut);
        }
    }

   

    private IEnumerator FadeIn(AudioSource aSource, float duration, float targetVolume)
    {
        float timer = 0f;
        float currentVolume = aSource.volume;
        float targetValue = Mathf.Clamp(targetVolume, minVolume, maxVolume);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float newVolume = Mathf.Lerp(currentVolume, targetValue, timer/duration);
            aSource. volume = newVolume;
            yield return null;
        }
    }

    private IEnumerator FadeOut(AudioSource aSource, float duration, float targetVolume)
    {
        float timer = 0f;
        float currentVolume = aSource.volume;
        float targetValue = Mathf.Clamp(targetVolume, minVolume, maxVolume);

        while (aSource.volume > 0)
        {
            timer += Time.deltaTime;
            float newVolume = Mathf.Lerp(currentVolume, targetValue, timer/duration);
            aSource.volume = newVolume;
            yield return null;
        }
    }
}


