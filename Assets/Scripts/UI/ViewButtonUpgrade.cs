using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ViewButtonUpgrade : MonoBehaviour
{
    private const float c_coefficientExperience = 0.01f; 
    private const int c_multiplierLevel = 10;

    [SerializeField] private Ability _ability;
    [SerializeField] private TextMeshProUGUI _countCoins;
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private float _startCoins = 25;
    [SerializeField] private float _levelCoefficientExperience = 0.7f;

    private Button _button;
    private float _cost;

    public event Action<float> OnButtonClicked;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _ability.LevelRaised += ChangeCost;
        _ability.LevelLimit += OnDisableButton;
        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _ability.LevelRaised -= ChangeCost;
        _ability.LevelLimit -= OnDisableButton;
        _button.onClick.RemoveListener(OnClick);        
    }    

    public void ChangeCost(int level)
    {
        int multiplier = level / c_multiplierLevel;
        float levelCoefficient = c_coefficientExperience * multiplier; 
        _cost = _startCoins + (_startCoins * level * 2);
        _cost = (_startCoins * Mathf.Pow(level, _levelCoefficientExperience + levelCoefficient) + (_startCoins * level));
        _level.text = level.ToString();
        _countCoins.text = NumberFormatter.Format(_cost);
    }

    private void OnClick()
    {
        OnButtonClicked?.Invoke(_cost);
    }

    private void OnDisableButton() => _button.enabled = false;
}
