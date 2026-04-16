using System;
using UnityEngine;
using UnityEngine.UI;

public class LeaderbordScreen : Window
{
    [Header("Button")]
    [SerializeField] private Button _levelButton;
    [SerializeField] private Button _coinsButton;
    [SerializeField] private Button _destroyBombsButton;

    [Header("Plane")]
    [SerializeField] private GameObject _levelPlanes;
    [SerializeField] private GameObject _coinsPlanes;
    [SerializeField] private GameObject _destroyBombsPlanes;

    public event Action OnReturButtonClicked;

    protected override void OnEnable()
    {
        base.OnEnable();
        _levelButton.onClick.AddListener(OnLevelPlane);
        _destroyBombsButton.onClick.AddListener(OnDestroyBombsPlane);
        _coinsButton.onClick.AddListener(OnCoinsPlane);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _levelButton.onClick.RemoveListener(OnLevelPlane);
        _destroyBombsButton.onClick.RemoveListener(OnDestroyBombsPlane);
        _coinsButton.onClick.RemoveListener(OnCoinsPlane);
    }

    public override void Open()
    {
        OnLevelPlane();
        base.Open();
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
        _destroyBombsPlanes.SetActive(false);
        _levelPlanes.SetActive(true);
    }

    private void OnDestroyBombsPlane()
    {
        _coinsPlanes.SetActive(false);
        _levelPlanes.SetActive(false);
        _destroyBombsPlanes.SetActive(true);
    }

    private void OnCoinsPlane()
    {
        _levelPlanes.SetActive(false);
        _destroyBombsPlanes.SetActive(false);
        _coinsPlanes.SetActive(true);
    }
}
