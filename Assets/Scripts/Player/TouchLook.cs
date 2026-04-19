using UnityEngine;

public class TouchLook : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public float sensitivity = 0.1f;

    int touchId = -1;
    Vector2 lastPos;

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                if (touch.position.x > Screen.width * 0.5f &&
                    !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    touchId = touch.fingerId;
                    lastPos = touch.position;
                }
            }

            if (touch.fingerId == touchId)
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 delta = touch.position - lastPos;
                    playerMovement.AddLookInput(delta * sensitivity);
                    lastPos = touch.position;
                }

                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                    touchId = -1;
            }
        }
    }
}