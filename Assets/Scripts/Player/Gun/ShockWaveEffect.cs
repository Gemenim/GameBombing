public class ShockWaveEffect : BulletEffect
{
    public void SetRadius(float diametr)
    {
        _particleSystem.startSize = diametr;
    }

    public void Play() => _particleSystem.Play();
}
