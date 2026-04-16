public class DamageExplosion : Ability
{
    private const float c_coefficientDamage = 0.02f;

    protected override void UpdateStatus()
    {
        float coefficientDamage = Level * c_coefficientDamage;
        _bulletObject.SetDamageExplosion(coefficientDamage);
    }
}
