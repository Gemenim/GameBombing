using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class Window : MonoBehaviour
{
    [SerializeField] private CanvasGroup _windowGroup;
    [SerializeField] private Button _closeButton;

    protected CanvasGroup WindowGroup => _windowGroup;
    protected Button ActionButtonOpen => _closeButton;

    public event Action OnClose;

    protected virtual void OnEnable()
    {
        _closeButton.onClick.AddListener(OnButtonCloseClick);
    }

    protected virtual void OnDisable()
    {
        _closeButton.onClick.RemoveListener(OnButtonCloseClick);
    }

    protected virtual void OnButtonCloseClick()
    {
        OnClose?.Invoke();
    }

    public abstract void Open();
    public abstract void Close();
}
