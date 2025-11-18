using UnityEngine;

public class ShockWaveEffect : BulletEffect
{
    public void SetDiametr(float diametr)
    {
        _particleSystem.startSize = diametr;
    }

    public void Play() => _particleSystem.Play();
}
