using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelBar : MonoBehaviour
{
    [SerializeField] private Image _bar;
    [SerializeField] private float _speedChange;
    [SerializeField] private Color _endColor;
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Animator _animator;

    private string _nameParametr = "Fullness";

    private float _needExperience = 100;
    private float _experience;
    private bool _isActive = false;

    public float Experience => _experience;

    public event Action OnButtonClicked;
    public event Action GainedExperience;
    public event Action FilledUp;

    private void Awake()
    {
        _button.enabled = false;
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClick);
    }

    public void SetNeedExperience(float value, int level)
    {
        _needExperience = value;
        _button.enabled = false;
        _bar.fillAmount = 0;
        _experience = 0;
        ChangeText(level);
    }

    public void OnButtonClick()
    {
        OnButtonClicked?.Invoke();
        _isActive = true;
        _button.enabled = false;
        _animator.SetFloat(_nameParametr, 0);
    }

    public void OnDisableButton()
    {
        _isActive = false;
    }

    public void AddExperience(float value)
    {
        _experience += value;
        GainedExperience?.Invoke();
        StartCoroutine(ChangeValue());

        if (_experience >= _needExperience)
        {
            _experience = _needExperience;
            FilledUp?.Invoke();

            if (_isActive == false)
            {
                _button.enabled = true;
                _animator.SetFloat(_nameParametr, _experience / _needExperience);
            }
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
}