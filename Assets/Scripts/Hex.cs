using UnityEngine;

[SelectionBase]
public class Hex : MonoBehaviour
{
    [SerializeField]
    private GlowHighlight highlight;
    private HexCoordinates _hexCoordinates;

    [SerializeField]
    private HexType hexType;

    public Vector3Int HexCoords => _hexCoordinates.GetHexCoords();

    public int GetCost()
        => hexType switch
        {
            HexType.Difficult => 20,
            HexType.Default => 10,
            HexType.Road => 5,
            _ => 100,
        };

    public bool IsObstacle()
    {
        return hexType == HexType.Obstacle;
    }

    private void Awake()
    {
        _hexCoordinates = GetComponent<HexCoordinates>();
        highlight = GetComponent<GlowHighlight>();
    }

    public void EnableHighlight()
    {
        highlight.ToggleGlow(true);
    }

    public void DisableHighlight()
    {
        highlight.ToggleGlow(false);
    }

    public void ResetHighlight()
    {
        highlight.ResetGlowHighlight();
    }

    public void HighlightPath()
    {
        highlight.HighlightValidPath();
    }
}
