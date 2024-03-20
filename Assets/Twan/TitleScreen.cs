using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TitleScreen : MonoBehaviour
{
    public VisualEffect pigeonVFXL;
    public VisualEffect pigeonVFXR;

    public AudioSource pigeonSound;

    // Start is called before the first frame update
    void Start()
    {
        pigeonVFXL.Stop();
        pigeonVFXR.Stop();  
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            pigeonVFXL.Play();
            pigeonVFXR.Play();
            pigeonSound.Play();
        }

    }
}
