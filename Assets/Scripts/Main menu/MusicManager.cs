using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource;

    // Szenen in denen Musik spielen soll
    string[] musicScenes = { "MainMenu", "Credits" };

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Prüfen ob aktuelle Szene in der Liste ist
        bool shouldPlay = System.Array.Exists(musicScenes, s => s == scene.name);

        if (shouldPlay && !audioSource.isPlaying)
            audioSource.Play();
        else if (!shouldPlay)
            audioSource.Stop();
    }
}