using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "Bullet/bullet", order = 51)]
public class BulletObject : ScriptableObject
{
    [SerializeField] private float _velocity;

    private float _damage = 4;
    private float _radiusExplosion;
    private float _explosionDamageCoefficient;
    private int _maxCountRicochet;
    private float _delayExplosion;

    public float Damage => _damage;
    public float Velocity => _velocity;
    public float RadiusExplosion => _radiusExplosion;
    public float ExplosionDamageCoefficient => _explosionDamageCoefficient;
    public int MaxCountRicochet => _maxCountRicochet;
    public float DelayExlosion => _delayExplosion;

    public void SetDamage(float value) => _damage = value;
    public void SetRicochet(int value) => _maxCountRicochet = value;
    public void SetRadius(float value) => _damage = value;
    public void SetDamageExplosion(float value) => _damage = value;
    public void SetSpeedExplosion(float value) => _damage = value;
}
