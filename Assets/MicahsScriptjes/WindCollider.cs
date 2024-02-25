using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCollider : MonoBehaviour
{
    [HideInInspector] public PathCreator pathCreator;
    public float colliderPoint;
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
                tunnelScript.currentPath = pathCreator;
                tunnelScript.SetPath(colliderPoint);
            }
        }
    }
}
