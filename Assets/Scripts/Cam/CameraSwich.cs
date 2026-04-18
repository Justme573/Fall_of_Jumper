using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public Camera cam1;
    public Camera cam2;

    private bool isCam1Active = true;

    void Start()
    {
        cam1.enabled = true;
        cam2.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            isCam1Active = !isCam1Active;

            cam1.enabled = isCam1Active;
            cam2.enabled = !isCam1Active;
        }
    }
}