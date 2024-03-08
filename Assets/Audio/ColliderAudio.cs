using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public float volumeAan;
    public float volumeUit;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            audioSource.volume = volumeAan;
            //audioSource.Play();
        }
    }

      void OnTriggerExit(Collider other)
    {
            audioSource.volume = volumeUit;      
    }

     IEnumerator LerpFunction(float endValue, float duration)
  {
    float time = 0;
    float startValue = audioSource.volume;
        while (time < duration)
    {
      audioSource.volume = Mathf.Lerp(startValue, endValue, time / duration);
      time += Time.deltaTime;
      yield return null;
    }
    audioSource.volume = endValue;
  }

}
