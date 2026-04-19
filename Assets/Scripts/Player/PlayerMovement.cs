using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public Transform playerCamera;
    public FixedJoystick joystick;
    public bool forceAndroidControls = false;

    float xRotation = 0f;
    CharacterController controller;
    Vector3 velocity;

    Vector2 moveInput;
    Vector2 lookInput;
    bool jumpPressed;

    public void AddLookInput(Vector2 delta)
    {
        lookInput = delta;
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed) jumpPressed = true;
    }

    void Update()
    {
        bool isAndroid = Application.platform == RuntimePlatform.Android || forceAndroidControls;

        // Look
        float mouseX, mouseY;
        if (isAndroid)
        {
            mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
            mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;
        }
        else
        {
            mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
            mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;
        }

        // Rotation mit NaN-Schutz
        if (!float.IsNaN(mouseX) && !float.IsNaN(mouseY))
        {
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        // Move
        Vector3 move;
        if (isAndroid)
            move = transform.right * joystick.Horizontal + transform.forward * joystick.Vertical;
        else
            move = transform.right * moveInput.x + transform.forward * moveInput.y;

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (jumpPressed && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpPressed = false;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move((move * speed + velocity) * Time.deltaTime);
        lookInput = Vector2.zero;
    }
}