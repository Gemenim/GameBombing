using System;
using UnityEngine;
using UnityEngine.UI;

public class HudScreen : MonoBehaviour
{
    [SerializeField] private Button _upgrade;
    [SerializeField] private Button _settings;
 
    public event Action OnUpgradeButtonClicked;
    public event Action OnSetingsButtonClicked;

    private void OnEnable()
    {
        _upgrade.onClick.AddListener(OnUpgradeButtonClick);
        _settings.onClick.AddListener(OnSettingsButtonClieck);
    }

    private void OnDisable()
    {
        _upgrade.onClick.RemoveListener(OnUpgradeButtonClick);
        _settings.onClick.RemoveListener(OnSettingsButtonClieck);
    }

    private void OnSettingsButtonClieck()
    {
        OnSetingsButtonClicked?.Invoke();        
    }

    private void OnUpgradeButtonClick()
    {
        OnUpgradeButtonClicked?.Invoke();
    }
}
