using UnityEngine;

public class SpeedAttack : Ability
{
    private const float c_coefficientRecharge = 0.04f;

    [SerializeField] private float _timeRecharge = 2.6f;

    public float TimeAttack => _timeRecharge;

    protected override void UpdateStatus()
    {
        _timeRecharge -= Level * c_coefficientRecharge;
    }
}
