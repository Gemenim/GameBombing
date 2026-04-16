public class RadiusExposion : Ability
{
    private const float c_defoltRadiusExplosion = 1f;
    private const float c_coefficientRadiusExplosion = 0.2f;

    public float Radius { get; private set; }

    protected override void UpdateStatus()
    {
        if (Level > 1)
        {
            Radius = c_defoltRadiusExplosion + (c_coefficientRadiusExplosion * Level);
            _bulletObject.SetRadius(Radius);
        }
        else 
        {
            Radius = 0;
            _bulletObject.SetRadius(Radius);
        }
    }
}