using TMPro;
using UnityEngine;

public class SistemFont : MonoBehaviour
{
    [SerializeField] private TMP_FontAsset fontAsset;

    private TextMeshProUGUI[] objects;

    private void OnValidate()
    {
        if (objects == null)
            objects = FindObjectsOfType<TextMeshProUGUI>();

        foreach (TextMeshProUGUI obj in objects)
        {
            obj.font = fontAsset;
        }
    }
}
