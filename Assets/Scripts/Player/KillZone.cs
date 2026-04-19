using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Option 1: Szene neu laden (respawn)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            // Option 2: Zum Checkpoint teleportieren
            // other.transform.position = respawnPoint.position;

            // Option 3: Spieler töten (falls du ein Health-System hast)
            // other.GetComponent<Health>().Die();
        }
    }
}