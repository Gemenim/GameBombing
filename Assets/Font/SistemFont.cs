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

        Debug.Log(objects.Length);

        foreach (TextMeshProUGUI obj in objects)
        {
            obj.font = fontAsset;
        }
    }
}
