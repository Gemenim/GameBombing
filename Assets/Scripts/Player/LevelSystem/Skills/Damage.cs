using UnityEngine;

public class Damage : Ability
{
    [SerializeField] private float _levelCoefficientDamage = 1.9f;

    [SerializeField] private float _defoltDamage = 1;

    public float AmountDamage { get; private set; }

    protected override void UpdateStatus()
    {
        AmountDamage = LevelCalculator.Calculat(_defoltDamage, _levelCoefficientDamage, Level);
        AmountDamage = AmountDamage > 0 ? AmountDamage : _defoltDamage;
        _bulletObject.SetDamage(AmountDamage);
    }
}
