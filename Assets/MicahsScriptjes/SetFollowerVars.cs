using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SetFollowerVars : MonoBehaviour
{
    private SplineAnimate splineAnimate;
    private TunnelScript tunnelScript;
    private void Awake()
    {
        // set SplineAnimate variables 
        tunnelScript = FindObjectOfType<TunnelScript>();
        splineAnimate = GetComponent<SplineAnimate>();
        splineAnimate.Container = tunnelScript.currentPath;
        splineAnimate.StartOffset = tunnelScript.tunnelPoint;
        splineAnimate.enabled = true;
    }
    void Update()
    {
        // failsave (probably isn't working properly :) )
        if (splineAnimate.NormalizedTime + tunnelScript.tunnelPoint >= 1)
        {
            splineAnimate.enabled = false;
        }
    }
}
