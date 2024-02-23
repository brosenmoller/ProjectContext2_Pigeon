using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float followSpeed;

    [Header("References")]
    [SerializeField] private CinemachineVirtualCamera followCamera;
    [SerializeField] private CinemachineFreeLook freeLookCamera;
    [SerializeField] private Transform followTarget;
    [SerializeField] private Transform lookAt;

    private Vector3 followOffset;
}
