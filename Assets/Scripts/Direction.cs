using System.Collections.Generic;
using UnityEngine;

public static class Direction
{
    private static readonly List<Vector3Int> DirectionsOffsetOdd = new()
    {
        new Vector3Int(-1, 0, 1),
        new Vector3Int(0, 0, 1),
        new Vector3Int(1, 0, 0),
        new Vector3Int(0, 0, -1),
        new Vector3Int(-1, 0, -1),
        new Vector3Int(-1, 0, 0)
    };

    private static readonly List<Vector3Int> DirectionsOffsetEven = new()
    {
        new Vector3Int(0, 0, 1),
        new Vector3Int(1, 0, 1),
        new Vector3Int(1, 0, 0),
        new Vector3Int(1, 0, -1),
        new Vector3Int(0, 0, -1),
        new Vector3Int(-1, 0, 0)
    };

    public static List<Vector3Int> GetDirectionList(int z)
        => z % 2 == 0 ? DirectionsOffsetEven : DirectionsOffsetOdd;
}
