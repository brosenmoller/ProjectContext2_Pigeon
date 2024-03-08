using PathCreation;
using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class WindTunnelScript : MonoBehaviour
{
    private SplineContainer splineContainer;
    private float pathLength;
    private GameObject[] pathColliders;
    [SerializeField] private GameObject colliderPrefab;
    private int colliderAmount;
    void Start()
    {
        // create colliders along the spline
        splineContainer = GetComponent<SplineContainer>();
        pathLength = splineContainer.CalculateLength();
        BoxCollider sphereCollider = colliderPrefab.GetComponent<BoxCollider>();
        colliderAmount = Mathf.RoundToInt( pathLength / ( sphereCollider.size.z * transform.localScale.z * colliderPrefab.transform.localScale.z ));
        pathColliders = new GameObject[colliderAmount];

        for (int i = 0; i < colliderAmount; i++)
        {
            float point = ((float)i / colliderAmount) + 0.5f / colliderAmount;
            if (splineContainer.Evaluate(point, out float3 pos, out float3 tangent, out float3 upVector))
            {
                Quaternion rotation = Quaternion.LookRotation(tangent, upVector);
                pathColliders[i] = Instantiate(colliderPrefab, pos, rotation, transform);
                WindCollider windCollider = pathColliders[i].GetComponent<WindCollider>();
                windCollider.colliderPoint = point;
                windCollider.splineContainer = splineContainer;
            }
        }
    }
}
 