using System.Collections;
using UnityEngine;

public enum BirdMovementMode
{
    Standard,
    Realistic,
    Tracking,
    Basic,
}

public class BirdMovement : MonoBehaviour
{
    [Header("Mode")]
    [SerializeField] private BirdMovementMode birdMovementMode;
    [SerializeField] private bool autoMove;

    [Header("General")]
    [SerializeField] private float maxY;

    [Header("Speed Control")]
    [SerializeField] private float startSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float minSpeed;
    [SerializeField] private float speedIncrease;
    [SerializeField] private float speedDecrease;
    [SerializeField] private float stoppingDeceleration;

    [Header("Rotation Control")]
    [SerializeField] private float verticalRotationSpeed;
    [SerializeField] private float horizontalRotationSpeed;

    [Header("Tracking")]
    [SerializeField] private float trackingSpeed;
    [SerializeField] private float maxLook;

    [Header("Boost")]
    [SerializeField] private float boostStrength;
    [SerializeField] private float boostAcceleration;
    [SerializeField] private float boostDuration;
    [SerializeField] private float boostDecceleration;
    [SerializeField] private float boostCooldown;

    [Header("Looks")]
    [SerializeField] private float bodyRotationSpeed;
    [SerializeField] private float bodyMaxRotation;

    [Header("References")]
    [SerializeField] private Transform body;
    public Transform head;

    private float horizontal;
    private float vertical;

    [Header("Debug")]
    public float velocity;

    private Vector2 look;

    private bool inBoost;
    private float boostVelocityIncrease;
    private float originalMaxSpeed;

    private bool isMoving = true;
    private bool CanMove => autoMove || isMoving;

    private void Awake()
    {
        velocity = startSpeed;
        originalMaxSpeed = maxSpeed;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameManager.InputManager.controls.GamePlay.Horizontal.performed += ctx => horizontal = ctx.ReadValue<float>();
        GameManager.InputManager.controls.GamePlay.Verical.performed += ctx => vertical = ctx.ReadValue<float>();
        GameManager.InputManager.controls.GamePlay.Horizontal.canceled += _ => horizontal = 0;
        GameManager.InputManager.controls.GamePlay.Verical.canceled += _ => vertical = 0;

        GameManager.InputManager.controls.GamePlay.CameraLook.performed += ctx => look = ctx.ReadValue<Vector2>();
        GameManager.InputManager.controls.GamePlay.CameraLook.canceled += _ => look = Vector2.zero;

        GameManager.InputManager.controls.GamePlay.Forward.performed += _ => isMoving = false;
        GameManager.InputManager.controls.GamePlay.Forward.canceled += _ => isMoving = true;

        GameManager.InputManager.controls.GamePlay.Boost.performed += _ => StartBoost();
    
    }

    private void Update()
    {
        switch (birdMovementMode)
        {
            case BirdMovementMode.Basic: BasicMovement(); break;
            case BirdMovementMode.Standard: StandardRotation(); CalculateVelocity(); break;
            case BirdMovementMode.Realistic: RealisticRotation(); CalculateVelocity(); break;
            case BirdMovementMode.Tracking: TrackingRotation(); CalculateVelocity(); break;
        }
    }

    private void StartBoost()
    {
        if (inBoost) { return; }
        StartCoroutine(Boost());
    }
        

    private void BasicMovement()
    {
        // Movement
        int multiplier = CanMove ? 1 : 0;
        transform.position += multiplier * startSpeed * Time.deltaTime * transform.forward;
        transform.Rotate(horizontal * horizontalRotationSpeed * Time.deltaTime * Vector3.up);
        transform.position += -1f * vertical * (horizontalRotationSpeed / 2) * Time.deltaTime * Vector3.up;

        // Body Rotation
        Quaternion targetRotation = Quaternion.Euler(vertical * bodyMaxRotation * -1, 180, horizontal * bodyMaxRotation);
        body.localRotation = Quaternion.RotateTowards(body.localRotation, targetRotation, bodyRotationSpeed);
    }

    private void StandardRotation()
    {
        transform.Rotate(Vector3.right, vertical * verticalRotationSpeed * Time.deltaTime, Space.Self);
        transform.Rotate(Vector3.up, horizontal * horizontalRotationSpeed * Time.deltaTime, Space.World);
    }

    private void RealisticRotation()
    {
        transform.Rotate(Vector3.right, vertical * verticalRotationSpeed * Time.deltaTime, Space.Self);
        transform.Rotate(Vector3.forward, -1f * horizontal * horizontalRotationSpeed * Time.deltaTime, Space.Self);
    }

    private void TrackingRotation()
    {
        float dotProduct = Vector3.Dot(Vector3.up, transform.forward);
        if ((dotProduct > maxLook && look.y < 0) || (dotProduct < -maxLook && look.y > 0) || (dotProduct < maxLook && dotProduct > -maxLook))
        {
            transform.Rotate(Vector3.right, -1f * look.y * trackingSpeed * Time.deltaTime, Space.Self);
        }
        
        transform.Rotate(Vector3.up, look.x * trackingSpeed * Time.deltaTime, Space.World);
    }

    private void CalculateVelocity()
    {
        if (!CanMove || (transform.position.y > maxY && Vector3.Dot(Vector3.up, transform.forward) > 0))
        {
            if (velocity > 0)
            {
                velocity -= stoppingDeceleration;
            }
        }
        else
        {
            DefaultVelocity();
        }

        transform.position += velocity * Time.deltaTime * transform.forward;
    }

    private void DefaultVelocity()
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

    private IEnumerator Boost()
    {
        if (velocity < minSpeed) { velocity = minSpeed; }

        boostVelocityIncrease = velocity * boostStrength;

        float velocityIncrease = 0;
        while (velocityIncrease < boostVelocityIncrease)
        {
            velocityIncrease += boostAcceleration * Time.deltaTime;
            velocity += boostAcceleration * Time.deltaTime;
            maxSpeed += boostAcceleration * Time.deltaTime;
            yield return null;
        }

        inBoost = true;

        yield return new WaitForSeconds(boostDuration);

        while (maxSpeed > originalMaxSpeed)
        {
            maxSpeed -= boostDecceleration * Time.deltaTime;
            yield return null;
        }

        maxSpeed = originalMaxSpeed;

        yield return new WaitForSeconds(boostCooldown);

        inBoost = false;
    }

    public void ResetVelocity()
    {
        velocity = 0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, maxY, transform.position.z), 10f);
    }
}
