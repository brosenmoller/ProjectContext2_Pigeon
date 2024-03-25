using UnityEngine;
using System.Collections;

public class JournalistController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnAroundDelay = 0.5f;
    [SerializeField] private Vector3 endPosition;

    [Header("Newspaper Settings")]
    [SerializeField] private Sprite newsPaperSprite;
    [SerializeField] private float newsPaperDuration;

    [Header("References")]
    [SerializeField] private GameObject visuals;

    private Collider triggerCollider;
    private Vector3 startPosition;
    private CutsceneManager cutsceneManager;
    private BirdMovement player;

    private bool isActive;

    private void Awake()
    {
        startPosition = transform.position;

        cutsceneManager = FindObjectOfType<CutsceneManager>();
        triggerCollider = GetComponent<Collider>();
        player = FindObjectOfType<BirdMovement>();

        visuals.SetActive(false);
        triggerCollider.enabled = false;
    }

    public void Activate()
    {
        isActive = true;
        visuals.SetActive(true);
        triggerCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BirdMovement _))
        {
            StopAllCoroutines();
            triggerCollider.enabled = false;
            visuals.SetActive(false);

            player.enabled = false;
            GameManager.UIViewManager.Show(typeof(NewsPaperView));
            NewsPaperView newspaperView = (NewsPaperView)GameManager.UIViewManager.GetView(typeof(NewsPaperView));
            newspaperView.SetNewsPaper(newsPaperSprite, newsPaperDuration, OnComplete);
        }
    }

    private void OnComplete()
    {
        player.enabled = true;
        GameManager.UIViewManager.Show(typeof(GameView));
        cutsceneManager.JournalistCompleted();
    }

    private IEnumerator MoveTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Debug.Log((transform.position - targetPosition).sqrMagnitude);

        while ((transform.position - targetPosition).sqrMagnitude > 0.1f)
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
