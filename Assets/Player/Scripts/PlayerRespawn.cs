using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent (typeof(BirdMovement))]
public class PlayerRespawn : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float minRespawnHeight;
    [SerializeField] private float respawnDelay;
    [SerializeField] private float respawnCheckStepSize = 100.0f;
    [SerializeField] private float respawnCheckRadius = 3.0f;

    [Header("Collision")]
    [SerializeField] private Vector3 point0;
    [SerializeField] private Vector3 point1;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask groundMask;

    [Header("References")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private Transform body;
    [SerializeField] private AudioObject deathSound;
    [SerializeField] private AudioObject respawnSound;

    private Collider[] colliderAllocation;
    private BirdMovement birdMovement;

    private bool isDying;

    private void Awake()
    {
        colliderAllocation = new Collider[20];
        birdMovement = GetComponent<BirdMovement>();
    }

    private void FixedUpdate()
    {
        if (Physics.OverlapCapsuleNonAlloc(transform.position + point0, transform.position + point1, radius, colliderAllocation, groundMask) > 0)
        {
            Death();
        }
    }

    private void Death()
    {
        if (isDying) { return; }

        deathSound.Play();

        isDying = true;
        body.gameObject.SetActive(false);
        Instantiate(deathParticles, transform.position, Quaternion.identity);

        birdMovement.enabled = false;
        Invoke(nameof(Respawn), respawnDelay);
    }

    private void Respawn()
    {
        respawnSound.Play();

        Vector3 oldPosition = transform.position;
        float respawnHeight = Mathf.Clamp(transform.position.y, minRespawnHeight, float.MaxValue);
        Vector3 newPosition = new(transform.position.x, respawnHeight, transform.position.z);

        while (Physics.OverlapSphere(newPosition, respawnCheckRadius).Length > 0){
            newPosition -= transform.forward * respawnCheckStepSize;
        }

        transform.position = newPosition;

        body.gameObject.SetActive(true);
        birdMovement.enabled = true;
        birdMovement.ResetVelocity();
        birdMovement.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        isDying = false;

        if (virtualCamera != null) { virtualCamera.OnTargetObjectWarped(transform, transform.position - oldPosition); }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, minRespawnHeight, transform.position.z), respawnCheckRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + point0, radius);
        Gizmos.DrawWireSphere(transform.position + point1, radius);
    }
}
