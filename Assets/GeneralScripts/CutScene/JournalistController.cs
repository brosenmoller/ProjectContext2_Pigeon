using UnityEngine;
using System.Collections;

public class JournalistController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnAroundDelay = 0.5f;
    [SerializeField] private Vector3 endPosition;
    
    private MeshRenderer meshRenderer;
    private Collider triggerCollider;
    private Vector3 startPosition;
    private CutsceneManager cutsceneManager;

    private bool isActive;

    private void Awake()
    {
        startPosition = transform.position;

        cutsceneManager = FindObjectOfType<CutsceneManager>();

        meshRenderer = GetComponent<MeshRenderer>();
        triggerCollider = GetComponent<Collider>();

        meshRenderer.enabled = false;
        triggerCollider.enabled = false;
    }

    public void Activate()
    {
        isActive = true;
        meshRenderer.enabled = true;
        triggerCollider.enabled = true;
        StartCoroutine(JournalistWalking());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BirdMovement _))
        {
            StopAllCoroutines();
            triggerCollider.enabled = false;
            meshRenderer.enabled = false;
            cutsceneManager.JournalistCompleted();
        }
    }

    private IEnumerator MoveTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;

        while ((transform.position - targetPosition).sqrMagnitude > 0.02f)
        {
            transform.Translate(moveSpeed * Time.deltaTime * direction);
            yield return null;
        }
    }

    private IEnumerator JournalistWalking()
    {
        while (isActive)
        {
            yield return MoveTowards(endPosition);
            yield return new WaitForSeconds(turnAroundDelay);

            yield return MoveTowards(startPosition);
            yield return new WaitForSeconds(turnAroundDelay);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(endPosition, 10f);
        Gizmos.DrawLine(transform.position, endPosition);
    }
}
