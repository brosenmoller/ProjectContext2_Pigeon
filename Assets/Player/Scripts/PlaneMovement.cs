using UnityEngine;

public class PlaneMovement : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float elevationSpeed;
    [SerializeField] private float rotationSpeed;

    [Header("Looks")]
    [SerializeField] private float bodyRotationSpeed;
    [SerializeField] private float bodyMaxRotation;

    [Header("References")]
    [SerializeField] private Transform body;

    private Controls controls;

    private float direction;
    private float elevation;

    private void Awake()
    {
        controls = new Controls();
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        controls.GamePlay.Horizontal.performed += ctx => direction = ctx.ReadValue<float>();
        controls.GamePlay.Verical.performed += ctx => elevation = ctx.ReadValue<float>();
        controls.GamePlay.Horizontal.canceled += _ => direction = 0;
        controls.GamePlay.Verical.canceled += _ => elevation = 0;
    }

    private void Update()
    {
        // Movement
        transform.position += -1f * movementSpeed * Time.deltaTime * transform.forward;
        transform.Rotate(direction * rotationSpeed * Time.deltaTime * Vector3.up);
        transform.position += -1f * elevation * elevationSpeed * Time.deltaTime * Vector3.up;

        // Body Rotation
        Quaternion targetRotation = Quaternion.Euler(elevation * bodyMaxRotation * -1, transform.localRotation.y, direction * bodyMaxRotation);
        body.localRotation = Quaternion.RotateTowards(body.localRotation, targetRotation, bodyRotationSpeed);
    }
}
