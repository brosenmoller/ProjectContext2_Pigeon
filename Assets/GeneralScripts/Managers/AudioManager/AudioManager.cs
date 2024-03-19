using System;
using System.Collections;
using UnityEngine;

public class AudioManager : Manager
{
    public IEnumerator FadeAudio(AudioSource audioSource, float duration, float targetVolume, Action onComplete = null)
    {
        float startVolume = audioSource.volume;
        float targetValue = Mathf.Clamp(targetVolume, 0, 1);

        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;

            audioSource.volume = Mathf.Lerp(startVolume, targetValue, Mathf.Pow(timer / duration, 2));

            yield return null;
        }

        if (audioSource.volume < 0.01f) audioSource.Stop();
        onComplete?.Invoke();
    }
}