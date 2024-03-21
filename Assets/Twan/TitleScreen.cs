using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TitleScreen : MonoBehaviour
{
    public AudioSource pigeonSound;

    void Start()
    {

    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            pigeonSound.Play();
        }

    }
}
