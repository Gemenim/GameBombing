using UnityEngine;
using YG;

[RequireComponent(typeof(LanguageYG))]
public class Translator : MonoBehaviour
{
    [SerializeField] private Language _language;

    private LanguageYG _languageYG;

    private void Awake()
    {
        _languageYG = GetComponent<LanguageYG>();
    }

    private void OnEnable()
    {
        _language.Translated += Translate;
    }

    private void OnDisable()
    {
        _language.Translated -= Translate;
    }

    private void Translate()
    {
        _languageYG.SwitchLanguage();
    }
}
