using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class ControllerAlhpaButtons : MonoBehaviour
{
    [SerializeField] RectTransform _contenet;

    private CanvasGroup _group;
    private SpriteRenderer[] _buttons;

    private void Awake()
    {
        _group = GetComponent<CanvasGroup>();
        _buttons = _contenet.GetComponentsInChildren<SpriteRenderer>();
    }

    public void ChangeAlpha()
    {
        foreach (SpriteRenderer button in _buttons) 
        {
            Color color = button.color;
            button.color = new Color(color.r, color.g, color.b, _group.alpha);
        }
    }
}
