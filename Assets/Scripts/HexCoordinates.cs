using UnityEngine;

public class HexCoordinates : MonoBehaviour
{
    private const float XOffset = 2, YOffset = 1, ZOffset = 1.73f;

    private Vector3Int offsetCoordinates;

    private void Awake()
    {
        offsetCoordinates = ConvertPositionToOffset(transform.position);
    }

    public static Vector3Int ConvertPositionToOffset(Vector3 position)
    {
        var x = Mathf.CeilToInt(position.x / XOffset);
        var y = Mathf.RoundToInt(position.y / YOffset);
        var z = Mathf.RoundToInt(position.z / ZOffset);

        return new Vector3Int(x, y, z);
    }

    public Vector3Int GetHexCoords() => offsetCoordinates;
}
