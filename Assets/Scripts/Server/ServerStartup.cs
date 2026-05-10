using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Collections;

public class ServerStartup : MonoBehaviour
{
    void Start()
    {
        #if UNITY_SERVER
        Debug.Log("Dedicated Server startet...");
        DontDestroyOnLoad(gameObject); // ← Das fehlte!
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("Multiplayermenu");
        #endif
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        #if UNITY_SERVER
        if (scene.name == "Multiplayermenu")
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            StartCoroutine(StartServerDelayed());
        }
        #endif
    }

    IEnumerator StartServerDelayed()
    {
        yield return new WaitUntil(() => NetworkManager.Singleton != null);
        Debug.Log("NetworkManager gefunden, starte Server!");
        
        // An alle Interfaces binden statt nur localhost
        var transport = (Unity.Netcode.Transports.UTP.UnityTransport)
            NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        transport.SetConnectionData("0.0.0.0", 7777, "0.0.0.0");
        
        NetworkManager.Singleton.StartServer();
        NetworkManager.Singleton.SceneManager.LoadScene("MultiplayerTest", LoadSceneMode.Single);
    }
}