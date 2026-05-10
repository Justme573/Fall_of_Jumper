using UnityEngine;
using Unity.Netcode;

public class ThirdPersonCamera : NetworkBehaviour
{
    public Transform target;
    public float distance = 4f;
    public float sensitivity = 3f;
    public float height = 1.5f;

    float yaw;
    float pitch;

    private Camera cam;

    public override void OnNetworkSpawn()
    {
        void Awake()
        {
            // Automatisch sich selbst als target setzen
            if (target == null)
                target = this.transform;
        }
        // Nur für den eigenen Spieler aktiv bleiben
        if (!IsOwner)
        {
            enabled = false;
            return;
        }

        // Eigene Kamera holen (nicht Camera.main!)
        cam = GetComponentInChildren<Camera>();
        if (cam == null)
        {
            // Fallback: Camera.main nur wenn ich der Owner bin
            cam = Camera.main;
        }
    }

    void Update()
    {
        if (!IsOwner) return;

        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, -30f, 70f);
    }

    void LateUpdate()
    {
        if (!IsOwner) return;
        if (!target || cam == null) return;

        transform.position = target.position + Vector3.up * height;

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);

        cam.transform.position = transform.position + offset;
        cam.transform.rotation = rotation;
    }
}