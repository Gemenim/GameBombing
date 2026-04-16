using UnityEngine;
using UnityEngine.UI;
using YG;

[RequireComponent(typeof(Button))]
public class ShowVidioButton : MonoBehaviour
{
    [SerializeField] private int _id;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(Show);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(Show);
    }

    private void Show()
    {
        YandexGame.RewVideoShow(_id);
    }
}
