using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
     void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            
           AudioController.instance.StartMusic();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            
           AudioController.instance.StopMusic();
        }
    }
}
