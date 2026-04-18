using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 4f;
    public float sensitivity = 3f;
    public float height = 1.5f;

    float yaw;
    float pitch;

    void Update()
    {
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;

        pitch = Mathf.Clamp(pitch, -30f, 70f);
    }

    void LateUpdate()
    {
        if (!target) return;

        // Pivot folgt Player
        transform.position = target.position + Vector3.up * height;

        // Rotation der Kamera (wichtig!)
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // Kamera Position
        Vector3 offset = rotation * new Vector3(0, 0, -distance);

        Camera.main.transform.position = transform.position + offset;

        // 👉 DAS ersetzt LookAt sauber
        Camera.main.transform.rotation = rotation;
    }
}