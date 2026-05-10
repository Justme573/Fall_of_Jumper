using UnityEngine;
using Unity.Netcode;

public class CameraSetup : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        // Nur eigene Kamera aktivieren, alle anderen deaktivieren
        GetComponent<Camera>().enabled = IsOwner;
        GetComponent<AudioListener>().enabled = IsOwner;
    }
}