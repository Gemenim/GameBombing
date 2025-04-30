using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private Transform _transform;
    private Rigidbody _rb;
    private float _damage = 4;
    private Pool<Bullet> _pool;
    private float _velocity;
    private Vector3 _lastVelocity;
    private float _radiusExplosion;
    private float _explosionDamageCoefficient;

    public float Damage => _damage;

    private void Awake()
    {
        _transform = transform;
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _lastVelocity = _rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        List<Cube> cubes = GetCubes();

        if (cubes.Count > 0)
        {
            Explosion(cubes);
        }

        if (collision.contacts.Length > 0)
        {
            Vector3 direction = Vector3.Reflect(_lastVelocity.normalized, collision.contacts[0].normal);
            _rb.velocity = direction * _velocity;
        }
    }

    public void SetDirection(Vector3 direction) => _rb.velocity = direction * _velocity;
    public void SetVelocity(float velocity) => _velocity = velocity;
    public void SetPool(Pool<Bullet> pool) => _pool = pool;
    public void ReturnInPool() => _pool.Return(this);

    public void SetStats(float damage, float radiusExplosion, float explosionDamageCoefficient)
    {
        _damage = damage;
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

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        Debug.Log(_radiusExplosion);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_transform.position, _radiusExplosion);
    }
}
