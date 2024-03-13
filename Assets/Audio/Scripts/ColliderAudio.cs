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
}
