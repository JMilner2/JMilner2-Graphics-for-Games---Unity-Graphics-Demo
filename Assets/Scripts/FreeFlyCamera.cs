using UnityEngine;

public class FreeFlyCamera: MonoBehaviour
{
    [Header("Camera Controls")]
    [SerializeField, Tooltip("Enable camera rotation by mouse movement")]
    private bool enableRotation = true;

    [SerializeField, Tooltip("Sensitivity of mouse rotation")]
    private float mouseSensitivity = 1.8f;

    [SerializeField, Tooltip("Enable camera movement by 'W','A','S','D','Q','E' keys")]
    private bool enableMovement = true;

    [SerializeField, Tooltip("Camera movement speed")]
    private float movementSpeed = 5f;

    [Header("Initialization")]
    [SerializeField, Tooltip("Move the camera to initialization position")]
    private KeyCode initPositionButton = KeyCode.R;

    private Vector3 initPosition;
    private Vector3 initRotation;

    private void Start()
    {
        initPosition = transform.position;
        initRotation = transform.eulerAngles;
    }

    private void Update()
    {
        SetCursorState();

        if (Cursor.visible)
            return;

        // Movement
        if (enableMovement)
        {
            Vector3 deltaPosition = Vector3.zero;

            deltaPosition += Input.GetKey(KeyCode.W) ? transform.forward : Vector3.zero;
            deltaPosition -= Input.GetKey(KeyCode.S) ? transform.forward : Vector3.zero;
            deltaPosition -= Input.GetKey(KeyCode.A) ? transform.right : Vector3.zero;
            deltaPosition += Input.GetKey(KeyCode.D) ? transform.right : Vector3.zero;
            deltaPosition += Input.GetKey(KeyCode.E) ? transform.up : Vector3.zero;
            deltaPosition -= Input.GetKey(KeyCode.Q) ? transform.up : Vector3.zero;

            transform.position += deltaPosition * movementSpeed * Time.deltaTime;
        }

        // Rotation
        if (enableRotation)
        {
            // Pitch and Yaw
            transform.rotation *= Quaternion.AngleAxis(-Input.GetAxis("Mouse Y") * mouseSensitivity, Vector3.right);
            transform.rotation = Quaternion.Euler(
                transform.eulerAngles.x,
                transform.eulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity,
                transform.eulerAngles.z
            );
        }

        // Return to init position
        if (Input.GetKeyDown(initPositionButton))
        {
            transform.position = initPosition;
            transform.eulerAngles = initRotation;
        }
    }

    // Apply requested cursor state
    private void SetCursorState()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Hide cursor when locking
        Cursor.visible = (Cursor.lockState != CursorLockMode.Locked);
    }
}
