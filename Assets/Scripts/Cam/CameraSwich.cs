using UnityEngine;
using Unity.Netcode;

public class CameraSwitch : NetworkBehaviour
{
    public Camera cam1;
    public Camera cam2;

    private bool isCam1Active = true;

    void Awake()
    {
        Debug.Log("CameraSwitch Awake! IsOwner: " + IsOwner);
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log($"=== CameraSwitch === IsOwner: {IsOwner} | IsLocalPlayer: {IsLocalPlayer} | ClientId: {NetworkManager.Singleton.LocalClientId}");

        if (cam1 == null)
            cam1 = transform.Find("Player Cam")?.GetComponent<Camera>();
        if (cam2 == null)
            cam2 = transform.Find("Pivot/3rd person")?.GetComponent<Camera>();

        if (cam1 == null)
        {
            Debug.LogError("cam1 ist NULL!");
            return;
        }

        Debug.Log($"cam1 gefunden: {cam1.name} | wird aktiviert: {IsOwner}");
        
        cam1.enabled = IsOwner;
        if (cam2 != null) cam2.enabled = false;

        if (cam1.GetComponent<AudioListener>())
            cam1.GetComponent<AudioListener>().enabled = IsOwner;
    }

    void Update()
    {
        if (!IsOwner) return;

        if (Input.GetKeyDown(KeyCode.F5))
        {
            isCam1Active = !isCam1Active;
            cam1.enabled = isCam1Active;
            if (cam2 != null) cam2.enabled = !isCam1Active;
        }
    }
}