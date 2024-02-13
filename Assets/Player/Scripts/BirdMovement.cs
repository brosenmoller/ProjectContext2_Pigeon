using UnityEngine;

public class BirdMovement : MonoBehaviour
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

    private int lastDirection = 0;
    private float timer = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        int direction = (int)Input.GetAxisRaw("Horizontal");
        int elevation = (int)Input.GetAxisRaw("Vertical");

        // Movement
        transform.position += -1f * movementSpeed * Time.deltaTime * transform.forward;
        transform.Rotate(direction * rotationSpeed * Time.deltaTime * Vector3.up);
        transform.position += -1f * elevation * elevationSpeed * Time.deltaTime * Vector3.up;

        // Body Rotation
        float sideRotation = Mathf.Lerp(lastDirection, direction, timer);
        timer += bodyRotationSpeed * Time.deltaTime;
        body.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y, sideRotation * bodyMaxRotation);

        lastDirection = direction;
    }
}
