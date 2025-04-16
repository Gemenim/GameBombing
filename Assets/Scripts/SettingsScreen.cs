using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScreen : Window
{
    public event Action OnReturnButtonClicked;
    public event Action OnExitButtonClicked;

    public override void Close()
    {
        WindowGroup.alpha = 0;
        WindowGroup.interactable = false;
        WindowGroup.blocksRaycasts = false;
    }

    public override void Open()
    {
        WindowGroup.alpha = 1.0f;
        WindowGroup.interactable = true;
        WindowGroup.blocksRaycasts = true;
    }

    protected override void OnButtonClick()
    {
        OnReturnButtonClicked?.Invoke();
        Close();
    }

    public void OnExitButtonClick()
    {
        OnExitButtonClicked?.Invoke();
    }
}
