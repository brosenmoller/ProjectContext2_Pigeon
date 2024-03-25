using UnityEngine;

[RequireComponent(typeof(BirdMovement), typeof(TunnelScript))]
public class PlayerAnimation : MonoBehaviour
{
    private const string DIVE = "Dive";

    [SerializeField] private Animator animator;

    private BirdMovement movement;
    private TunnelScript tunnelScript;
    private Transform head;

    private void Awake()
    {
        movement = GetComponent<BirdMovement>();
        tunnelScript = GetComponent<TunnelScript>();
        head = movement.head;
    }

    private void Update()
    {
        if (movement.transform.position.y > head.position.y + 1.0f || tunnelScript.insideTunnel)
        {
            animator.SetBool(DIVE, true);
        }
        else
        {
            animator.SetBool(DIVE, false);
        }
    }
}

