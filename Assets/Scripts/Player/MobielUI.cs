using UnityEngine;

public class MobileUI : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(
            Application.platform == RuntimePlatform.Android || 
            Application.platform == RuntimePlatform.IPhonePlayer
        );
    }
}