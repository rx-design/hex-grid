using UnityEngine;
using UnityEngine.Events;

public class SelectionManager : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask selectionMask;

    public UnityEvent<GameObject> UnitSelected;
    public UnityEvent<GameObject> TerrainSelected;

    public void HandleClick(Vector3 mousePosition)
    {
        if (!FindTarget(mousePosition, out var target))
        {
            return;
        }

        if (IsUnit(target))
        {
            UnitSelected.Invoke(target);
        }
        else
        {
            TerrainSelected.Invoke(target);
        }
    }

    private bool FindTarget(Vector3 mousePosition, out GameObject target)
    {
        var ray = mainCamera.ScreenPointToRay(mousePosition);

        target = Physics.Raycast(ray, out var hitInfo, 100, selectionMask)
            ? hitInfo.collider.gameObject
            : null;

        return target != null;
    }

    private static bool IsUnit(GameObject target)
    {
        return target.GetComponent<Unit>() != null;
    }
}
