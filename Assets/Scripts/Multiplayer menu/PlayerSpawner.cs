using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;
public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform[] spawnPoints;
    private int spawnIndex = 0;
    private HashSet<ulong> alreadySpawned = new HashSet<ulong>();
    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += SpawnPlayer;

        if (NetworkManager.Singleton.IsServer)
        {
            #if !UNITY_SERVER
            // Nur im Editor/Host einen eigenen Spieler spawnen
            SpawnPlayer(NetworkManager.Singleton.LocalClientId);
            #endif
        }
    }

    void SpawnPlayer(ulong clientId)
    {
        if (!NetworkManager.Singleton.IsServer) return;
        if (alreadySpawned.Contains(clientId)) return;

        // Auf Dedicated Server keinen Spieler für den Server selbst spawnen
        #if UNITY_SERVER
        if (clientId == NetworkManager.Singleton.LocalClientId) return;
        #endif

        alreadySpawned.Add(clientId);

        Vector3 spawnPos = new Vector3(spawnIndex * 5, 5, 0);
        spawnIndex++;

        Debug.Log($"Spawne Client {clientId} an {spawnPos}");
        GameObject player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    }

    void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.OnClientConnectedCallback -= SpawnPlayer;
    }
}