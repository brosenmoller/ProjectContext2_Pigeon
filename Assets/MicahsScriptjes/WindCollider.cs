using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class WindCollider : MonoBehaviour
{
    [HideInInspector] public SplineContainer splineContainer;
    [HideInInspector] public float colliderPoint;
    void Start()
    {
    }
    // check if player is inside tunnel
    private void OnTriggerEnter(Collider col)
    {
        if (col.TryGetComponent(out TunnelScript tunnelScript))
        {
            if(!tunnelScript.insideTunnel && !tunnelScript.onCooldown)
            {
                tunnelScript.insideTunnel = true;
                tunnelScript.currentPath = splineContainer;
                tunnelScript.SetPath(colliderPoint);
            }
        }
    }
}
