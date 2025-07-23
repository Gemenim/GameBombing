using System;
using UnityEngine;
using YG;

public class Language : MonoBehaviour
{
    public event Action Translated;

    public void Translat(string language)
    {
        YandexGame.lang = language;
        Translated?.Invoke();
    }
}
