using System;
using UnityEngine;
using UnityEngine.UI;

public class HudScreen : MonoBehaviour
{
    [SerializeField] private Button _upgrade;
    [SerializeField] private Button _settings;
    [SerializeField] private Button _liderbord;
 
    public event Action OnUpgradeButtonClicked;
    public event Action OnSetingsButtonClicked;
    public event Action OnLiderbordButtonClicked;

    private void OnEnable()
    {
        _upgrade.onClick.AddListener(OnUpgradeButtonClick);
        _settings.onClick.AddListener(OnSettingsButtonClick);
        _liderbord.onClick.AddListener(OnLiderbordButtonClick);
    }

    private void OnDisable()
    {
        _upgrade.onClick.RemoveListener(OnUpgradeButtonClick);
        _settings.onClick.RemoveListener(OnSettingsButtonClick);
        _liderbord.onClick.RemoveListener(OnLiderbordButtonClick);
    }

    private void OnLiderbordButtonClick()
    {
        OnLiderbordButtonClicked?.Invoke();
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
