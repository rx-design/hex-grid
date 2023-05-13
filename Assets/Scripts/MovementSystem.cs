using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementSystem : MonoBehaviour
{
    private SearchResult _movementRange;
    private List<Vector3Int> _currentPath = new();

    public void ShowRange(Unit selectedUnit, HexGrid hexGrid)
    {
        CalculateRange(selectedUnit, hexGrid);

        var unitPos = hexGrid.GetClosestHex(selectedUnit.transform.position);

        foreach (var hexPosition in _movementRange.GetRangePositions())
        {
            if (unitPos == hexPosition) continue;
            hexGrid.GetTileAt(hexPosition).EnableHighlight();
        }
    }

    public void HideRange(HexGrid hexGrid)
    {
        foreach (var hexPosition in _movementRange.GetRangePositions())
        {
            hexGrid.GetTileAt(hexPosition).DisableHighlight();
        }
    }

    public void ShowPath(Vector3Int selectedHexPosition, HexGrid hexGrid)
    {
        if (!_movementRange.GetRangePositions().Contains(selectedHexPosition))
        {
            return;
        }

        foreach (var hexPosition in _currentPath)
        {
            hexGrid.GetTileAt(hexPosition).ResetHighlight();
        }

        _currentPath = _movementRange.GetPathTo(selectedHexPosition);

        foreach (var hexPosition in _currentPath)
        {
            hexGrid.GetTileAt(hexPosition).HighlightPath();
        }
    }

    private void CalculateRange(Unit selectedUnit, HexGrid hexGrid)
    {
        var closestHex = hexGrid.GetClosestHex(selectedUnit.transform.position);
        var movementPoints = selectedUnit.MovementPoints;

        _movementRange = GraphSearch.GetRange(hexGrid, closestHex, movementPoints);
    }

    public void MoveUnit(Unit selectedUnit, HexGrid hexGrid)
    {
        var tiles = _currentPath.Select(position => hexGrid.GetTileAt(position).transform.position);

        selectedUnit.MoveThroughPath(tiles);
    }

    public bool IsHexInRange(Vector3Int hexPosition)
    {
        return _movementRange.IsHexPositionInRange(hexPosition);
    }
}
