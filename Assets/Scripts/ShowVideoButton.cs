using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using YG;

[RequireComponent(typeof(Button))]
public class ShowVideoButton : MonoBehaviour
{
    [SerializeField] private int _id = 0;

    private Button _button;

    private void OnValidate()
    {
        if (_id < 0)
            _id = 0;
    }

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
        YandexGame.RewVideoShow(_id);
    }
}
