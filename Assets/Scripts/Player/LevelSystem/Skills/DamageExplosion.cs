public class DamageExplosion : Ability
{
    private const float c_coefficientDamage = 0.02f;

    public float CoefficientDamage { get; private set; }

    protected override void UpdateStatus()
    {
        CoefficientDamage = Level * c_coefficientDamage;
    }
}
