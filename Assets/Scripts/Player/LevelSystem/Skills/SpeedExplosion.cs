public class SpeedExplosion : Ability
{
    private const float c_defoltRechargeExplosion = 30f;
    private const float c_stepSpeedExplosion = 0.3f;

    public float DaleyExplosion { get; private set; }

    protected override void UpdateStatus()
    {
        DaleyExplosion = c_defoltRechargeExplosion - c_stepSpeedExplosion * Level;
        _bulletObject.SetSpeedExplosion(DaleyExplosion);
    }
}
