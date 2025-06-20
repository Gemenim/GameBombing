using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ViewButtonUpgrade : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countCoins;
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private double _startCoins = 25;
    [SerializeField] private float _levelCoefficientExperience = 0.7f;

    private Button _button;
    private double _cost;

    public event Action<double> OnButtonClicked;

    private void OnValidate()
    {
        ChangeText(1);
    }

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);        
    }    

    public void ChangeText(int level)
    {
        _cost = _startCoins + (_startCoins * level * 2);
        _cost = (_startCoins * Mathf.Pow(level, _levelCoefficientExperience) +(_startCoins * level));
        _level.text = level.ToString();
        _countCoins.text = NumberFormatter.Format(_cost);
    }

    private void OnClick()
    {
        OnButtonClicked?.Invoke(_cost);
    }
}
