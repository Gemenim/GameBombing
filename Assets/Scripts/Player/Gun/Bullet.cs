using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(TrailRenderer))]
public class Bullet : MonoBehaviour
{
    private Transform _transform;
    private Rigidbody _rb;
    private AudioSource _audioSource;
    private float _damage = 4;
    private Pool<Bullet> _pool;
    private float _velocity;
    private Vector3 _lastVelocity;
    private float _radiusExplosion;
    private float _explosionDamageCoefficient;
    private int _levelRicochet = 1;
    private int _countRicochet = 0;

    public float Damage => _damage;

    private void Awake()
    {
        _transform = transform;
        _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        _lastVelocity = _rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        List<Cube> cubes = GetCubes();
        _audioSource.PlayOneShot(_audioSource.clip);
        _countRicochet++;

        if (cubes.Count > 0)
        {
            Explosion(cubes);
        }

        if (collision.contacts.Length > 0)
        {
            Vector3 direction = Vector3.Reflect(_lastVelocity.normalized, collision.contacts[0].normal);
            _rb.velocity = direction * _velocity;
        }

        if (_countRicochet == _levelRicochet + 1)
        {
            ReturnInPool();
            return;
        }
    }

    public void SetDirection(Vector3 direction) => _rb.velocity = direction * _velocity;
    public void SetVelocity(float velocity) => _velocity = velocity;
    public void SetPool(Pool<Bullet> pool) => _pool = pool;

    public void ReturnInPool()
    {
        _countRicochet = 0;
        _pool.Return(this);
    }


    public void SetStats(float damage, int levelRicochet, float radiusExplosion, float explosionDamageCoefficient)
    {
        _damage = damage;
        _levelRicochet = levelRicochet;
        _radiusExplosion = radiusExplosion;
        _explosionDamageCoefficient = explosionDamageCoefficient;
    }

    private void Explosion(List<Cube> cubes)
    {
        foreach (Cube cube in cubes)
        {
            float distans = Vector3.Distance(_transform.position, cube.transform.position);
            float damage = _damage * _explosionDamageCoefficient * (distans / _radiusExplosion);
            cube.TakeDamage(damage);
        }
    }

    private List<Cube> GetCubes()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _radiusExplosion);
        List<Cube> cubes = new();

        foreach (Collider hit in hits)
            if (hit.TryGetComponent<Cube>(out Cube cube))
                cubes.Add(cube);

        return cubes;
    }
}
