using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SystemChangeFonts : MonoBehaviour
{
    [SerializeField] private TMP_FontAsset _font;

    private TextMeshProUGUI[] _texts;

    private void OnValidate()
    {
        if (_texts == null)
            _texts = FindObjectsOfType<TextMeshProUGUI>();

        Debug.Log(_texts.Length);

        if (_texts.Length > 0)
        {
            foreach (TextMeshProUGUI text in _texts)
                text.font = _font;
        }
    }
}
