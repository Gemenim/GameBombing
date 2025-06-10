using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewLevelBar : MonoBehaviour
{
    [SerializeField] private Image _bar;
    [SerializeField] private float _speedChange;
    [SerializeField] private Color _endColor;
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _text;

    private Color _startColor;
    private Color _targetColor;
    private bool _isOnButton = false;

    private double _needExperience = 100;
    private double _experience;
    private bool _isActive = false;
    private Coroutine _coroutine;

    public double Experience => _experience;

    public event Action OnButtonClicked;

    private void Awake()
    {
        _button.enabled = false;
        _startColor = _bar.color;
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    public void SetNeedExperience(double value, int level)
    {
        _needExperience = value;
        _button.enabled = false;
        _bar.fillAmount = 0;
        _experience = 0;
        ChangeText(level);
    }

    public void OnButtonClick()
    {
        _isOnButton = false;
        OnButtonClicked?.Invoke();
        _isActive = true;
        StopCoroutine(_coroutine);
        _bar.color = _startColor;
        _button.enabled = false;
    }

    public void OnDisableButton()
    {
        _isActive = false;
    }

    public void SetValue(double value)
    {
        _experience += value;

        if (_experience >= _needExperience)
        {
            _experience = _needExperience;

            if (_isActive == false)
            {
                _button.enabled = true;
                _isOnButton = true;
                _coroutine = StartCoroutine(Animate());
            }
        }
        else
        {
            StartCoroutine(ChangeValue());
        }
    }

    private void ChangeText(int count)
    {
        _text.text = count.ToString();
    }

    private IEnumerator ChangeValue()
    {
        while (_experience / _needExperience != _bar.fillAmount)
        {
            _bar.fillAmount = Mathf.MoveTowards(_bar.fillAmount, (float)(_experience / _needExperience), Time.deltaTime * _speedChange);

            yield return null;
        }
    }

    private IEnumerator Animate()
    {
        while (_isOnButton)
        {
            _bar.color = Color.Lerp(_startColor, _targetColor, Mathf.PingPong(Time.time, 1));

            yield return null;
        }
    }
}