using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScreen : Window
{
    [SerializeField] private Button _resetSeve;

    public event Action OnReturnButtonClicked;
    public event Action OnResetSaveButtonClicked;
    public event Action OnExitButtonClicked;

    protected override void OnEnable()
    {
        base.OnEnable();
        _resetSeve.onClick.AddListener(OnResetSaveButtonClick);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _resetSeve.onClick.RemoveListener(OnResetSaveButtonClick);
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

    public void OnResetSaveButtonClick()
    {
        OnResetSaveButtonClicked?.Invoke();
    }

    public void OnExitButtonClick()
    {
        OnExitButtonClicked?.Invoke();
    }
}
