using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera followCamera;

    private void Awake()
    {
        followCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 direction = followCamera.transform.position - transform.position;
        Vector3 flatDirection = Vector3.ProjectOnPlane(direction, Vector3.up);
        transform.forward = flatDirection;
    }
}
