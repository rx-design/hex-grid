using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    private readonly Dictionary<Vector3Int, Hex> _hexTileDict = new();
    private readonly Dictionary<Vector3Int, List<Vector3Int>> _hexTileNeighboursDict = new();

    public Hex[] hexes;

    private void Start()
    {
        foreach (var hex in hexes)
        {
            _hexTileDict[hex.HexCoords] = hex;
        }
    }

    public Hex GetTileAt(Vector3Int hexCoords)
    {
        return _hexTileDict.TryGetValue(hexCoords, out var hex) ? hex : null;
    }

    public List<Vector3Int> GetNeighboursFor(Vector3Int hexCoordinates)
    {
        if (_hexTileDict.ContainsKey(hexCoordinates) == false)
        {
            return new List<Vector3Int>();
        }

        if (_hexTileNeighboursDict.TryGetValue(hexCoordinates, out var neighbours))
        {
            return neighbours;
        }

        _hexTileNeighboursDict.Add(hexCoordinates, new List<Vector3Int>());

        foreach (var direction in Direction.GetDirectionList(hexCoordinates.z))
        {
            if (_hexTileDict.ContainsKey(hexCoordinates + direction))
            {
                _hexTileNeighboursDict[hexCoordinates].Add(hexCoordinates + direction);
            }
        }

        return _hexTileNeighboursDict[hexCoordinates];
    }

    public Vector3Int GetClosestHex(Vector3 worldPosition)
    {
        worldPosition.y = 0;

        return HexCoordinates.ConvertPositionToOffset(worldPosition);
    }
}
