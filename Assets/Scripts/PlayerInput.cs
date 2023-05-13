using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    public UnityEvent<Vector3> pointerClick;

    private void Update()
    {
        DetectMouseClick();
    }

    private void DetectMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePosition = Input.mousePosition;
            pointerClick.Invoke(mousePosition);
        }
    }
}
