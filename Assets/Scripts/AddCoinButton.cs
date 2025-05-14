using UnityEngine;
using UnityEngine.UI;
using YG;

[RequireComponent(typeof(Button))]
public class AddCoinButton : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    protected virtual void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    protected virtual void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        YandexGame.RewVideoShow(0);
    }
}
