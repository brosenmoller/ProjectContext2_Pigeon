using PathCreation;
using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class WindTunnelScript : MonoBehaviour
{
    private PathCreator pathCreator;
    private float pathLength;
    private GameObject[] pathColliders;
    [SerializeField] private GameObject colliderPrefab;
    void Start()
    {
        // create box colliders along path
        pathCreator = GetComponent<PathCreator>();
        pathLength = pathCreator.path.length;
        BoxCollider sphereCollider = colliderPrefab.GetComponent<BoxCollider>();
        int colliderAmount = Mathf.RoundToInt(pathLength / sphereCollider.size.z);
        pathColliders = new GameObject[colliderAmount];
        for (int i = 0; i < colliderAmount; i++)
        {
            float point = (pathLength / colliderAmount) * i;
            pathColliders[i] = Instantiate(colliderPrefab, pathCreator.path.GetPointAtDistance(point), pathCreator.path.GetRotationAtDistance(point), transform);
            WindCollider windCollider = colliderPrefab.GetComponent<WindCollider>();
            windCollider.pathCreator = pathCreator;
            windCollider.colliderPoint = point;
            if (i == colliderAmount - 1)
            {
                windCollider.colliderPoint = 0;
            }
        }
    }
}
 