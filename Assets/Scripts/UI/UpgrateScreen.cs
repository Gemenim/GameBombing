using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgrateScreen : Window
{
    [SerializeField] private Button _upAttack;
    [SerializeField] private Button _upRadiusExplosion;
    [SerializeField] private Button _upDamageExplosion;

    [SerializeField] private double _countCoinsDamage;
    [SerializeField] private double _countCoinsRadiusExplosion;
    [SerializeField] private double _countCoinsDamageExplosion;

    public event Action OnReturnButtonClicked;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    public override void Close()
    {
        WindowGroup.alpha = 0;
        WindowGroup.interactable = false;
        WindowGroup.blocksRaycasts = false;
    }

    public override void Open()
    {
        WindowGroup.alpha = 1.0f;
        WindowGroup.interactable = true;
        WindowGroup.blocksRaycasts = true;
    }

    protected override void OnButtonClick()
    {
        OnReturnButtonClicked?.Invoke();
        Close();
    }
}
