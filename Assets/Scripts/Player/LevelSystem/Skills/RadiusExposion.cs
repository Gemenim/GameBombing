public class RadiusExposion : Ability
{
    private const float c_defoltRadiusExplosion = 0.1f;
    private const float c_coefficientRadiusExplosion = 0.2f;

    public float Radius { get; private set; }

    protected override void UpdateStatus()
    {
        Radius = c_defoltRadiusExplosion + (c_coefficientRadiusExplosion * Level);
    }
}