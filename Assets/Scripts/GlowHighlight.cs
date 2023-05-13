using System.Collections.Generic;
using UnityEngine;

public class GlowHighlight : MonoBehaviour
{
    private static readonly int GlowColor = Shader.PropertyToID("_GlowColor");
    private readonly Dictionary<Renderer, Material[]> _glowMaterialDictionary = new();
    private readonly Dictionary<Renderer, Material[]> _originalMaterialDictionary = new();
    private readonly Dictionary<Color, Material> _cachedGlowMaterials = new();
    private readonly Color _validSpaceColor = Color.green;

    public Material glowMaterial;

    private Color _originalGlowColor;
    private bool _isGlowing;

    private void Awake()
    {
        PrepareMaterialDictionaries();
        _originalGlowColor = glowMaterial.GetColor(GlowColor);
    }

    private void PrepareMaterialDictionaries()
    {
        foreach (var childRenderer in GetComponentsInChildren<Renderer>())
        {
            var originalMaterials = childRenderer.materials;
            _originalMaterialDictionary.Add(childRenderer, originalMaterials);

            var newMaterials = new Material[childRenderer.materials.Length];
            for (var i = 0; i < originalMaterials.Length; i++)
            {
                if (_cachedGlowMaterials.TryGetValue(originalMaterials[i].color, out var mat) == false)
                {
                    mat = new Material(glowMaterial)
                    {
                        color = originalMaterials[i].color
                    };
                    _cachedGlowMaterials[mat.color] = mat;
                }
                newMaterials[i] = mat;
            }
            _glowMaterialDictionary.Add(childRenderer, newMaterials);
        }
    }

    private void ToggleGlow()
    {
        if (_isGlowing == false)
        {
            ResetGlowHighlight();

            foreach (var theRenderer in _originalMaterialDictionary.Keys)
            {
                theRenderer.materials = _glowMaterialDictionary[theRenderer];
            }
        }
        else
        {
            foreach (var theRenderer in _originalMaterialDictionary.Keys)
            {
                theRenderer.materials = _originalMaterialDictionary[theRenderer];
            }
        }

        _isGlowing = !_isGlowing;
    }

    public void ToggleGlow(bool state)
    {
        if (_isGlowing == state)
        {
            return;
        }

        _isGlowing = !state;
        ToggleGlow();
    }

    public void ResetGlowHighlight()
    {
        var renderers = _glowMaterialDictionary.Keys;

        foreach (var r in renderers)
        {
            var materials = _glowMaterialDictionary[r];

            foreach (var m in materials)
            {
                m.SetColor(GlowColor, _originalGlowColor);
            }
        }
    }

    public void HighlightValidPath()
    {
        if (!_isGlowing)
        {
            return;
        }

        var renderers = _glowMaterialDictionary.Keys;

        foreach (var r in renderers)
        {
            var materials = _glowMaterialDictionary[r];

            foreach (var m in materials)
            {
                m.SetColor(GlowColor, _validSpaceColor);
            }
        }
    }
}
