using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
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

        if (!IsOwner) return; // Nur eigene Kamera sperren
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnMove(InputValue value)
    {
        if (!IsOwner) return;
        moveInput = value.Get<Vector2>();
    }

    void OnLook(InputValue value)
    {
        if (!IsOwner) return;
        lookInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!IsOwner) return;
        if (value.isPressed) jumpPressed = true;
    }

    void Update()
    {
        if (!IsOwner) return; // Alles nur für eigenen Spieler!

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        bool isAndroid = Application.platform == RuntimePlatform.Android || forceAndroidControls;

        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        if (!float.IsNaN(mouseX) && !float.IsNaN(mouseY))
        {
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        Vector3 move;
        if (isAndroid)
            move = transform.right * joystick.Horizontal + transform.forward * joystick.Vertical;
        else
            move = transform.right * moveInput.x + transform.forward * moveInput.y;

        if (jumpPressed && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpPressed = false;
        }

        controller.Move(move * speed * Time.deltaTime);
        lookInput = Vector2.zero;
    }
}