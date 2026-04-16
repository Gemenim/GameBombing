using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class Window : MonoBehaviour
{
    [SerializeField] private CanvasGroup _windowGroup;
    [SerializeField] private Button _closeButton;

    protected CanvasGroup WindowGroup => _windowGroup;

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

    public virtual void Open()
    {
        WindowGroup.alpha = 1f;
        WindowGroup.interactable = true;
        WindowGroup.blocksRaycasts = true;
    }

    public virtual void Close()
    {
        WindowGroup.alpha = 0;
        WindowGroup.interactable = false;
        WindowGroup.blocksRaycasts = false;
    }
}
