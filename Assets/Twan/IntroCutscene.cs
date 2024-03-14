using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroCutscene : MonoBehaviour
{
    public string nextScene;
    public Animator introAnimator;
    int pressCount;
    bool isTiming;

    //textAnimators
    public Animator text1, text2, text3, text3a, text4, text5, text5a, text6;

    //differentiate public and private float in order to reset internal float to inspector value
    public float pressTimer = 60f;
    float internalTimer;

    void Start(){
        //start true to allow player to take in panels
        isTiming = true;
        internalTimer = pressTimer;
    }

    void Update(){
        if(Input.GetMouseButtonDown(0) && isTiming == false){
            pressCount++;
            isTiming = true;

            introAnimator.SetTrigger("to" + pressCount);
        }

        Debug.Log(pressCount);

        //set timer so click can't be spammed
        if(isTiming == true && internalTimer > 0) {
            internalTimer--;
        }
        else {
            isTiming = false;
            internalTimer = pressTimer;
        }

        if(pressCount == 1) {
            text1.SetTrigger("play");
        }
        if(pressCount == 2) {
            text2.SetTrigger("play");
        }
        if(pressCount == 3) {
            text3.SetTrigger("play");
            text3a.SetTrigger("play");
        }
        if(pressCount == 4) {
            text4.SetTrigger("play");
        }
        if(pressCount == 5) {
            text5.SetTrigger("play");
            text5a.SetTrigger("play");
        }
        if(pressCount == 6) {
            text6.SetTrigger("play");
        }
    }
}
