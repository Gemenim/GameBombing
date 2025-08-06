using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class HudScreen : MonoBehaviour
{
    [SerializeField] private Button _save;
    [SerializeField] private Button _upgrade;
    [SerializeField] private Button _settings;
    [SerializeField] private Button _leaderbord;
    [SerializeField] private Image _timer;
    [SerializeField] private Window[] _windows;

    private CanvasGroup _group;

    public event Action OnSaveButtonClicked;
    public event Action OnUpgradeButtonClicked;
    public event Action OnSetingsButtonClicked;
    public event Action OnLeaderbordButtonClicked;

    private void Awake()
    {
        _group = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        _save.onClick.AddListener(OnSaveButtonClick);
        _upgrade.onClick.AddListener(OnUpgradeButtonClick);
        _settings.onClick.AddListener(OnSettingsButtonClick);
        _leaderbord.onClick.AddListener(OnLeaderbordButtonClick);

        foreach (Window window in _windows)
            window.OnClose += On;
    }

    private void OnDisable()
    {
        _save.onClick.RemoveListener(OnSaveButtonClick);
        _upgrade.onClick.RemoveListener(OnUpgradeButtonClick);
        _settings.onClick.RemoveListener(OnSettingsButtonClick);
        _leaderbord.onClick.RemoveListener(OnLeaderbordButtonClick);

        foreach (Window window in _windows)
            window.OnClose += On;
    }

    public void OnTimer() => _timer.gameObject.SetActive(true);
    public void OffTimer() => _timer.gameObject.SetActive(false);

    private void OnSaveButtonClick()
    {
        OnSaveButtonClicked?.Invoke();
    }

    private void OnSettingsButtonClick()
    {
        _group.interactable = false;
        _group.blocksRaycasts = false;
        OnSetingsButtonClicked?.Invoke();        
    }

    private void OnUpgradeButtonClick()
    {
        _group.interactable = false;
        _group.blocksRaycasts = false;
        OnUpgradeButtonClicked?.Invoke();
    }

    private void OnLeaderbordButtonClick()
    {
        _group.interactable = false;
        _group.blocksRaycasts = false;
        OnLeaderbordButtonClicked?.Invoke();
    }

    private void On()
    {
        _group.interactable = true;
        _group.blocksRaycasts = true;
    }
}
