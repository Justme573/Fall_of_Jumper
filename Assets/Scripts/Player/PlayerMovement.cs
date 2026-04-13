using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float mouseSensitiyity = 100f;
    
    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    public Transform playerCamera;
    float xRotation = 0f;
    CharacterController controller;
    Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

void Update()
{
    // Mouse Look
    float mouseX = Input.GetAxis("Mouse X") * mouseSensitiyity * Time.deltaTime;
    float mouseY = Input.GetAxis("Mouse Y") * mouseSensitiyity * Time.deltaTime;

    xRotation -= mouseY;
    xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    transform.Rotate(Vector3.up * mouseX);

    // Movement Input
    float x = Input.GetAxis("Horizontal");
    float z = Input.GetAxis("Vertical");

    Vector3 move = transform.right * x + transform.forward * z;

    // Ground check fix
    if (controller.isGrounded && velocity.y < 0)
    {
        velocity.y = -2f;
    }

    // Jump
    if (Input.GetButtonDown("Jump") && controller.isGrounded)
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    // Like Gravity
    velocity.y += gravity * Time.deltaTime;

    // 🔥 COMBINE EVERYTHING
    Vector3 finalMove = move * speed + velocity;

    controller.Move(finalMove * Time.deltaTime);
}
}