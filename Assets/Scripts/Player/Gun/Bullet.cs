using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private const float c_radius = 0.5f;

    [SerializeField] private Transform _shockWavePosition;
    [SerializeField] private ParticleSystem _shockWave;

    private Transform _transform;
    private Rigidbody _rb;
    private AudioSource _audioSource;
    private float _damage = 4;
    private Pool<Bullet> _pool;
    private float _velocity;
    private Vector3 _lastVelocity;
    private float _radiusExplosion;
    private float _explosionDamageCoefficient;
    private int _defoltRicochet = 5;
    private int _levelRicochet = 1;
    private int _countRicochet = 0;
    private float _delayExlosion;
    private Coroutine _rechargeExplosion;
    private bool _isCharged = true;

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
        Collider collider = collision.GetContact(0).otherCollider;

        if (collider.TryGetComponent<Cube>(out Cube cube))
        {
            cube.TakeDamage(_damage);

            if (_delayExlosion <= 0)
            {
                Debug.Log("Folse");
                List<Cube> cubes = GetCubes();

                if (cubes.Count > 0)
                    Explosion(cubes);
            }
            else
            {
                Debug.Log("GO");
                if (_isCharged)
                {
                    Debug.Log("True");
                    _rechargeExplosion = StartCoroutine(RechargeExlosion());
                    List<Cube> cubes = GetCubes();

                    if (cubes.Count > 0)
                        Explosion(cubes);
                }
            }
        }

        if (collision.contacts.Length > 0)
        {
            Vector3 direction = Vector3.Reflect(_lastVelocity.normalized, collision.contacts[0].normal);
            _rb.velocity = direction * _velocity;
        }

        _audioSource.PlayOneShot(_audioSource.clip);

        _countRicochet++;        

        if (_countRicochet == _levelRicochet + _defoltRicochet)
            ReturnInPool();
    }

    public void SetDirection(Vector3 direction) => _rb.velocity = direction * _velocity;
    public void SetVelocity(float velocity) => _velocity = velocity;
    public void SetPool(Pool<Bullet> pool) => _pool = pool;
    public void SetAudioSource(AudioSource audioSource) => _audioSource = audioSource;

    public void ReturnInPool()
    {
        if (!_isCharged)
        {
            StopCoroutine(_rechargeExplosion);
            _isCharged = true;
        }

        _shockWave.transform.parent = null;
        _countRicochet = 0;
        _pool.Return(this);
    }

    public void SetStats(float damage, int levelRicochet, float radiusExplosion, float explosionDamageCoefficient, float delayExplosion)
    {
        SetShockWaveEffect();
        _damage = damage;
        _levelRicochet = levelRicochet;
        _radiusExplosion = radiusExplosion + c_radius;
        _explosionDamageCoefficient = explosionDamageCoefficient;
        _delayExlosion = delayExplosion;
        _shockWave.startSize = _radiusExplosion * 2;
    }

    private void SetShockWaveEffect()
    {
        _shockWave.transform.parent = _shockWavePosition;
        _shockWave.transform.localPosition = Vector3.zero;
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
