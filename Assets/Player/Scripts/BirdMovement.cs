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
    [SerializeField] private Transform head;

    private float horizontal;
    private float vertical;

    [Header("Debug")]
    [SerializeField] private float velocity;

    private Vector2 look;

    private bool inBoost;
    private float boostVelocityIncrease;

    private bool isMoving = false;


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

        GameManager.InputManager.controls.GamePlay.CameraLook.performed += ctx => look = ctx.ReadValue<Vector2>();
        GameManager.InputManager.controls.GamePlay.CameraLook.canceled += _ => look = Vector2.zero;

        GameManager.InputManager.controls.GamePlay.Forward.performed += _ => isMoving = true;
        GameManager.InputManager.controls.GamePlay.Forward.canceled += _ => isMoving = false;

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
        transform.position += startSpeed * Time.deltaTime * transform.forward * CanMove();
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
        transform.Rotate(Vector3.right, -1f * look.y * trackingSpeed * Time.deltaTime, Space.Self);
        transform.Rotate(Vector3.up, look.x * trackingSpeed * Time.deltaTime, Space.World);
    }

    private void CalculateVelocity()
    {
        if (!autoMove && !isMoving)
        {
            velocity -= stoppingDeceleration;
        }
        else
        {
            DefaultVelocity();
        }

        transform.position += velocity * Time.deltaTime * transform.forward * CanMove();
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

    private int CanMove()
    {
        if (autoMove || isMoving) { return 1; }
        else { return 0; }
    }

    private IEnumerator Boost()
    {
        boostVelocityIncrease = velocity * boostStrength;
        
        float originalMaxSpeed = maxSpeed;

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

        yield return new WaitForSeconds(boostCooldown);

        inBoost = false;
     
    }

    public void ResetVelocity()
    {
        velocity = 0f;
    }
}
