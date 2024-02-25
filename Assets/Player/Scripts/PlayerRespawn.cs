using Cinemachine;
using UnityEngine;

[RequireComponent (typeof(BirdMovement))]
public class PlayerRespawn : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float respawnHeight;
    [SerializeField] private float respawnDelay;

    [Header("Collision")]
    [SerializeField] private Vector3 point0;
    [SerializeField] private Vector3 point1;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask groundMask;

    [Header("References")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private Transform body;

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

        isDying = true;
        body.gameObject.SetActive(false);
        Instantiate(deathParticles, transform.position, Quaternion.identity);

        birdMovement.enabled = false;
        Invoke(nameof(Respawn), respawnDelay);
    }

    private void Respawn()
    {
        Vector3 oldPosition = transform.position; 
        transform.position = new Vector3(transform.position.x, respawnHeight, transform.position.z);

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
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, respawnHeight, transform.position.z), 3f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + point0, radius);
        Gizmos.DrawWireSphere(transform.position + point1, radius);
    }
}
