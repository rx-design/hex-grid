using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField]
    private HexGrid hexGrid;
    [SerializeField]
    private MovementSystem movementSystem;
    [SerializeField]
    private Unit selectedUnit;

    private Hex _selectedHex;
    private bool _canMove = true;

    public void HandleUnitSelected(GameObject unit)
    {
        if (!_canMove)
        {
            return;
        }

        var unitInstance = unit.GetComponent<Unit>();

        if (IsUnitSelected(unitInstance))
        {
            return;
        }

        PrepareUnitForMovement(unitInstance);
    }

    public void HandleTerrainSelected(GameObject hex)
    {
        if (selectedUnit == null || !_canMove)
        {
            return;
        }

        var hexInstance = hex.GetComponent<Hex>();
        var coordinates = hexInstance.HexCoords;

        if (IsOutOfRange(coordinates) || IsUnitHex(coordinates))
        {
            return;
        }

        HandleHexSelected(hexInstance);
    }

    private bool IsUnitSelected(Object unit)
    {
        var isSelected = selectedUnit == unit;

        if (isSelected)
        {
            ClearSelection();
        }

        return isSelected;
    }

    private void ClearSelection()
    {
        selectedUnit.Deselect();
        movementSystem.HideRange(hexGrid);
        selectedUnit = null;
        _selectedHex = null;
    }

    private bool IsOutOfRange(Vector3Int position)
    {
        return movementSystem.IsHexInRange(position) == false;
    }

    private bool IsUnitHex(Vector3Int position)
    {
        if (position != hexGrid.GetClosestHex(selectedUnit.transform.position))
        {
            return false;
        }

        selectedUnit.Deselect();
        ClearSelection();

        return true;
    }

    private void HandleHexSelected(Hex hex)
    {
        if (_selectedHex == null || _selectedHex != hex)
        {
            _selectedHex = hex;
            movementSystem.ShowPath(hex.HexCoords, hexGrid);
        }
        else // double click
        {
            movementSystem.MoveUnit(selectedUnit, hexGrid);
            _canMove = false;
            selectedUnit.MovementFinished += OnMovementFinished;
            ClearSelection();
        }
    }

    private void OnMovementFinished(Unit unit)
    {
        unit.MovementFinished -= OnMovementFinished;
        _canMove = true;
    }

    private void PrepareUnitForMovement(Unit unit)
    {
        if (selectedUnit != null)
        {
            ClearSelection();
        }

        selectedUnit = unit;
        selectedUnit.Select();

        movementSystem.ShowRange(selectedUnit, hexGrid);
    }
}
