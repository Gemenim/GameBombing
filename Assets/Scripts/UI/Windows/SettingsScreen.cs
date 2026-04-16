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

    protected override void OnButtonCloseClick()
    {
        base.OnButtonCloseClick();
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
