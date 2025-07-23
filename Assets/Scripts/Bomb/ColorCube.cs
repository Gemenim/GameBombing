using UnityEngine;

public class ColorCube : MonoBehaviour
{
    [SerializeField] private Color _color;

    public Color Color => _color;

    private void Awake()
    {
        ApplyColor(_color);
    }

    private void OnValidate()
    {
        ApplyColor(_color);
    }

    public void ApplyColor(Color color)
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetColor("_Color", color);
        renderer.SetPropertyBlock(propertyBlock);
    }
}
