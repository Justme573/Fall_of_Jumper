using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public void BackMult()
    {
        SceneManager.LoadScene("MainMenu");
    }
}