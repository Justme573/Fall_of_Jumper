using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using TMPro;

public class MultiplayerMenu : MonoBehaviour
{
    public TMP_InputField ipInputField;

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        // Host lädt die Szene für ALLE über Netcode
        NetworkManager.Singleton.SceneManager.LoadScene("Multiplayertest", LoadSceneMode.Single);
    }

    public void StartClient()
    {
        string ip = ipInputField.text;
        if (string.IsNullOrEmpty(ip) || ip == "") ip = "127.0.0.1";

        var transport = (Unity.Netcode.Transports.UTP.UnityTransport)NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        transport.SetConnectionData(ip, 7777);

        NetworkManager.Singleton.StartClient();
        // Client lädt NICHT selbst - der Host schickt den Befehl!
    }
}