using System;
using UnityEngine;
using UnityEngine.UI;

public class LeaderbordScreen : Window
{
    [Header("Button")]
    [SerializeField] private Button _levelButton;
    [SerializeField] private Button _coinsButton;
    [SerializeField] private Button _destoyBombsButton;

    [Header("Plane")]
    [SerializeField] private GameObject _levelPlanes;
    [SerializeField] private GameObject _coinsPlanes;
    [SerializeField] private GameObject _destoyBombsPlanes;

    public event Action OnReturButtonClicked;

    protected override void OnEnable()
    {
        base.OnEnable();
        _levelButton.onClick.AddListener(OnLevelPlane);
        _destoyBombsButton.onClick.AddListener(OnDestoyBombsPlane);
        _coinsButton.onClick.AddListener(OnCoinsPlane);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _levelButton.onClick.RemoveListener(OnLevelPlane);
        _destoyBombsButton.onClick.RemoveListener(OnDestoyBombsPlane);
        _coinsButton.onClick.RemoveListener(OnCoinsPlane);
    }

    public override void Close()
    {
        WindowGroup.alpha = 0;
        WindowGroup.interactable = false;
        WindowGroup.blocksRaycasts = false;
    }

    public override void Open()
    {
        WindowGroup.alpha = 1f;
        WindowGroup.interactable = true;
        WindowGroup.blocksRaycasts = true;
    }

    protected override void OnButtonCloseClick()
    {
        base.OnButtonCloseClick();
        OnReturButtonClicked?.Invoke();
        Close();
    }

    private void OnLevelPlane()
    {
        _coinsPlanes.SetActive(false);
        _destoyBombsPlanes.SetActive(false);
        _levelPlanes.SetActive(true);
    }

    private void OnDestoyBombsPlane()
    {
        _coinsPlanes.SetActive(false);
        _levelPlanes.SetActive(false);
        _destoyBombsPlanes.SetActive(true);
    }

    private void OnCoinsPlane()
    {
        _levelPlanes.SetActive(false);
        _destoyBombsPlanes.SetActive(false);
        _coinsPlanes.SetActive(true);
    }
}
