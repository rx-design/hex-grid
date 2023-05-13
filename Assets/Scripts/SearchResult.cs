using System.Collections.Generic;
using UnityEngine;

public struct SearchResult
{
    public Dictionary<Vector3Int, Vector3Int?> VisitedNodesDict;

    public List<Vector3Int> GetPathTo(Vector3Int destination)
    {
        return VisitedNodesDict.ContainsKey(destination)
            ? GraphSearch.GeneratePath(destination, VisitedNodesDict)
            : new List<Vector3Int>();
    }

    public bool IsHexPositionInRange(Vector3Int position)
    {
        return VisitedNodesDict.ContainsKey(position);
    }

    public IEnumerable<Vector3Int> GetRangePositions()
        => VisitedNodesDict.Keys;
}
