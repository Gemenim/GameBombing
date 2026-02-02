using UnityEngine;

public class Damage : Ability
{
    private const float c_levelCoefficientDamage = 1.9f;

    [SerializeField] private float _defoltDamage = 1;

    public float AmountDamage { get; private set; }

    protected override void UpdateStatus()
    {
        AmountDamage = LevelCalculator.Calculat(_defoltDamage, c_levelCoefficientDamage, Level);
        AmountDamage = AmountDamage > 0 ? AmountDamage : _defoltDamage;
    }
}
