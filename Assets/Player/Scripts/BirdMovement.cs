using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float startSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float minSpeed;
    [SerializeField] private float speedIncrease;
    [SerializeField] private float speedDecrease;
    [SerializeField] private float verticalRotationSpeed;
    [SerializeField] private float horizontalRotationSpeed;

    [Header("Looks")]
    [SerializeField] private float bodyRotationSpeed;
    [SerializeField] private float bodyMaxRotation;

    [Header("References")]
    [SerializeField] private Transform body;
    [SerializeField] private Transform head;

    private float horizontal;
    private float vertical;

    [Header("Debug")]
    [SerializeField] private float velocity;

    private void Awake()
    {
        velocity = startSpeed;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameManager.InputManager.controls.GamePlay.Horizontal.performed += ctx => horizontal = ctx.ReadValue<float>();
        GameManager.InputManager.controls.GamePlay.Verical.performed += ctx => vertical = ctx.ReadValue<float>();
        GameManager.InputManager.controls.GamePlay.Horizontal.canceled += _ => horizontal = 0;
        GameManager.InputManager.controls.GamePlay.Verical.canceled += _ => vertical = 0;
    }

    private void Update()
    {
        // Body Rotation
        transform.Rotate(Vector3.up, horizontal * horizontalRotationSpeed * Time.deltaTime, Space.Self);
        transform.Rotate(Vector3.right, vertical * horizontalRotationSpeed * Time.deltaTime, Space.Self);

        CalculateVelocity();

        // Apply Velocity
        transform.position += velocity * Time.deltaTime * transform.forward;

        // Body Rotation
        Quaternion targetRotation = Quaternion.Euler(body.localRotation.x, 180, horizontal * bodyMaxRotation);
        body.localRotation = Quaternion.RotateTowards(body.localRotation, targetRotation, bodyRotationSpeed);
    }

    private void CalculateVelocity()
    {
        float distanceTillStraight = Mathf.Abs(transform.rotation.x - 90);
        float speedChangeMultiplier = Mathf.InverseLerp(0, 90, distanceTillStraight);
        
        if (transform.position.y > head.position.y)
        {
            velocity += speedIncrease * speedChangeMultiplier;
        }
        else
        {
            velocity -= speedDecrease * speedChangeMultiplier;
        }

        if (velocity > maxSpeed)
        {
            velocity = maxSpeed;
        }
        else if (velocity < minSpeed)
        {
            velocity = minSpeed;
        }
    }
}
