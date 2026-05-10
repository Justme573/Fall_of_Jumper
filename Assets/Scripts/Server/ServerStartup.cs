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
        NetworkManager.Singleton.StartServer();
        NetworkManager.Singleton.SceneManager.LoadScene("MultiplayerTest", LoadSceneMode.Single);
    }
}