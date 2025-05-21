using System;
using UnityEngine;
using UnityEngine.UI;

public class HudScreen : MonoBehaviour
{
    [SerializeField] private Button _save;
    [SerializeField] private Button _upgrade;
    [SerializeField] private Button _settings;

    public event Action OnSaveButtonClicked;
    public event Action OnUpgradeButtonClicked;
    public event Action OnSetingsButtonClicked;

    private void OnEnable()
    {
        _save.onClick.AddListener(OnSaveButtonClick);
        _upgrade.onClick.AddListener(OnUpgradeButtonClick);
        _settings.onClick.AddListener(OnSettingsButtonClick);
    }

    private void OnDisable()
    {
        _save.onClick.RemoveListener(OnSaveButtonClick);
        _upgrade.onClick.RemoveListener(OnUpgradeButtonClick);
        _settings.onClick.RemoveListener(OnSettingsButtonClick);
    }

    private void OnSaveButtonClick()
    {
        OnSaveButtonClicked?.Invoke();
    }

    private void OnSettingsButtonClick()
    {
        OnSetingsButtonClicked?.Invoke();        
    }

    private void OnUpgradeButtonClick()
    {
        OnUpgradeButtonClicked?.Invoke();
    }
}
