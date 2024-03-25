using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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

        GameManager.InputManager.controls.GamePlay.Horizontal.performed += SetHorizontal;
        GameManager.InputManager.controls.GamePlay.Verical.performed += SetVertical;
        GameManager.InputManager.controls.GamePlay.Horizontal.canceled += ResetHorizontal;
        GameManager.InputManager.controls.GamePlay.Verical.canceled += ResetVertical;

        GameManager.InputManager.controls.GamePlay.CameraLook.performed += SetLook;
        GameManager.InputManager.controls.GamePlay.CameraLook.canceled += ResetLook;

        GameManager.InputManager.controls.GamePlay.Forward.performed += ResetIsMoving;
        GameManager.InputManager.controls.GamePlay.Forward.canceled += SetIsMoving;

        GameManager.InputManager.controls.GamePlay.Boost.performed += StartBoost;
    }

    private void OnDestroy()
    {
        GameManager.InputManager.controls.GamePlay.Horizontal.performed -= SetHorizontal;
        GameManager.InputManager.controls.GamePlay.Verical.performed -= SetVertical;
        GameManager.InputManager.controls.GamePlay.Horizontal.canceled -= ResetHorizontal;
        GameManager.InputManager.controls.GamePlay.Verical.canceled -= ResetVertical;

        GameManager.InputManager.controls.GamePlay.CameraLook.performed -= SetLook;
        GameManager.InputManager.controls.GamePlay.CameraLook.canceled -= ResetLook;

        GameManager.InputManager.controls.GamePlay.Forward.performed -= ResetIsMoving;
        GameManager.InputManager.controls.GamePlay.Forward.canceled -= SetIsMoving;

        GameManager.InputManager.controls.GamePlay.Boost.performed -= StartBoost;
    }

    private void SetHorizontal(InputAction.CallbackContext ctx) => horizontal = ctx.ReadValue<float>();
    private void SetVertical(InputAction.CallbackContext ctx) => vertical = ctx.ReadValue<float>();
    private void ResetHorizontal(InputAction.CallbackContext ctx) => horizontal = 0;
    private void ResetVertical(InputAction.CallbackContext ctx) => vertical = 0;
    private void SetLook(InputAction.CallbackContext ctx) => look = ctx.ReadValue<Vector2>();
    private void ResetLook(InputAction.CallbackContext ctx) => look = Vector2.zero;

    private void SetIsMoving(InputAction.CallbackContext ctx) => isMoving = true;
    private void ResetIsMoving(InputAction.CallbackContext ctx) => isMoving = false;
    private void StartBoost(InputAction.CallbackContext callbackContext)
    {
        if (inBoost) { return; }
        StartCoroutine(Boost());
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
        if (Input.GetKey(KeyCode.Mouse2))
        {
            velocity = 1000f;
            return;
        }

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
        inBoost = true;

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
