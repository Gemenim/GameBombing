using System;
using UnityEngine;
using UnityEngine.UI;

public class HudScreen : MonoBehaviour
{
    [SerializeField] private Button _save;
    [SerializeField] private Button _upgrade;
    [SerializeField] private Button _settings;
    [SerializeField] private Button _leaderbord;
    [SerializeField] private Image _timer;

    public event Action OnSaveButtonClicked;
    public event Action OnUpgradeButtonClicked;
    public event Action OnSetingsButtonClicked;
    public event Action OnLeaderbordButtonClicked;

    private void OnEnable()
    {
        _save.onClick.AddListener(OnSaveButtonClick);
        _upgrade.onClick.AddListener(OnUpgradeButtonClick);
        _settings.onClick.AddListener(OnSettingsButtonClick);
        _leaderbord.onClick.AddListener(OnLeaderbordButtonClick);
    }

    private void OnDisable()
    {
        _save.onClick.RemoveListener(OnSaveButtonClick);
        _upgrade.onClick.RemoveListener(OnUpgradeButtonClick);
        _settings.onClick.RemoveListener(OnSettingsButtonClick);
        _leaderbord.onClick.RemoveListener(OnLeaderbordButtonClick);
    }

    public void OnTimer() => _timer.gameObject.SetActive(true);
    public void OffTimer() => _timer.gameObject.SetActive(false);

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

    private void OnLeaderbordButtonClick()
    {
        OnLeaderbordButtonClicked?.Invoke();
    }
}
