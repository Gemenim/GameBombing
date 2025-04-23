using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ViewButtonUpgrade : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countCoins;
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private double _startCoins = 10;

    private Button _button;
    private double _coins;

    public event Action<double> OnButtonClicked;

    private void Awake()
    {
        _button = GetComponent<Button>();
        ChangeText(1);
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _button.onClick.AddListener(OnClick);        
    }

    public void ChangeText(int level)
    {
        _coins = _startCoins + (_startCoins * level * 2);
        _level.text = level.ToString();
        _countCoins.text = _coins.ToString();
    }

    private void OnClick()
    {
        OnButtonClicked?.Invoke(_coins);
    }
}
