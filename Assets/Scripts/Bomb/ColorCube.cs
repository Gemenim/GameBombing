using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCube : MonoBehaviour
{
    [SerializeField] private Color _color;

    private void Awake()
    {
        ApplyColor();
    }

    private void OnValidate()
    {
        ApplyColor();        
    }

    private void ApplyColor()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetColor("_Color", _color);
        renderer.SetPropertyBlock(propertyBlock);
    }
}
