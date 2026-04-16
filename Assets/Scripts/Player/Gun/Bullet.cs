using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private const float c_radius = 0.5f;

    [SerializeField] private BulletObject _bulletSO;
    [SerializeField] private ShockWaveEffect _shockWave;
    [SerializeField] private BulletEffect _trail;

    private Transform _transform;
    private Rigidbody _rb;
    private AudioSource _audioSource;
    private float _damage = 4;
    private Pool<Bullet> _pool;
    private float _velocity;
    private float _radiusExplosion;
    private float _explosionDamageCoefficient;
    private int _maxCountRicochet;
    private int _countRicochet = 0;
    private float _delayExlosion;
    private Coroutine _rechargeExplosion;
    private bool _isCharged = true;

    private void Awake()
    {
        _transform = transform;
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _trail.Return += ReturnPool;
    }

    private void OnDisable()
    {
        _trail.Return -= ReturnPool;
    }

    private void FixedUpdate()
    {
        if (Vector3.Magnitude(_rb.velocity) < _velocity)
            _rb.AddForce(_velocity * _rb.velocity.normalized);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.GetContact(0).otherCollider;

        if (collider.TryGetComponent<Cube>(out Cube cube) && !cube.IsDetouch)
        {
            _countRicochet++;
            cube.TakeDamage(_damage);
        
            if (_delayExlosion <= 0)
            {
                List<Cube> cubes = GetCubes();

                if (cubes.Count > 0)
                    Explosion(cubes);
            }
            else
            {
                if (_isCharged && gameObject.activeSelf)
                {
                    _rechargeExplosion = StartCoroutine(RechargeExlosion());
                    List<Cube> cubes = GetCubes();

                    if (cubes.Count > 0)
                        Explosion(cubes);
                }
            }
        }

        _audioSource.PlayOneShot(_audioSource.clip);

        if (_countRicochet == _maxCountRicochet)
            Diseble();
    }

    public void SetDirection(Vector3 direction) => _rb.velocity = direction * _velocity;
    public void SetPool(Pool<Bullet> pool) => _pool = pool;
    public void SetAudioSource(AudioSource audioSource) => _audioSource = audioSource;

    public void Diseble()
    {
        if (!_isCharged)
        {
            StopCoroutine(_rechargeExplosion);
            _isCharged = true;
        }

        _countRicochet = 0;
        _trail.UnfastenIt();
        _shockWave.UnfastenIt();
        gameObject.SetActive(false);
    }

    public void SetStatus()
    {
        _damage = _bulletSO.Damage;
        _maxCountRicochet = _bulletSO.MaxCountRicochet;
        _explosionDamageCoefficient = _bulletSO.ExplosionDamageCoefficient;
        _radiusExplosion = _bulletSO.RadiusExplosion + c_radius;
        _velocity = _bulletSO.Velocity;
        _delayExlosion = _bulletSO.DelayExlosion;
        _shockWave.SetRadius(_radiusExplosion);
        _rb.maxLinearVelocity = _velocity;
    }

    private void ReturnPool()
    {
        _pool.Return(this);
    }

    private void Explosion(List<Cube> cubes)
    {
        Vector3 position = _transform.position;
        _shockWave.Play();

        foreach (Cube cube in cubes)
        {
            float radiusCube = cube.transform.localScale.x / 2;
            float distans = Vector3.Distance(position, cube.transform.position) - radiusCube;

            if (distans <= _radiusExplosion)
            {
                float damage = _damage * _explosionDamageCoefficient * (1 - (distans / _radiusExplosion));
                cube.TakeDamage(damage);
            }
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

    private IEnumerator RechargeExlosion()
    {
        WaitForSeconds delay = new WaitForSeconds(_delayExlosion);
        _isCharged = false;

        yield return delay;

        _isCharged = true;
    }
}
