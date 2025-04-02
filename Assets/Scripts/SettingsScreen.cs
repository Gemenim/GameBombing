using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScreen : Window
{
    [SerializeField] private Button _exit;

    public event Action OnReturnButtonClicked;
    public event Action OnExitButtonClicked;

    protected override void OnEnable()
    {
        base.OnEnable();
        _exit.onClick.AddListener(OnExitButtonClick);
    }

    protected override void OnDisable()
    {
        _exit.onClick.RemoveListener(OnExitButtonClick);        
    }

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

    protected void OnExitButtonClick()
    {
        Close();
        OnExitButtonClicked?.Invoke();
    }
}
