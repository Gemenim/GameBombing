using System;
using UnityEngine;
using YG;

[RequireComponent(typeof(AudioSource))]
public class Gun : MonoBehaviour
{
    [SerializeField] private Pool<Bullet> _pool;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Bullet _prefab;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private float _velocity;
    [SerializeField] private float _damage;
    [SerializeField] private double _startCostShot = 1;

    [SerializeField] private float _rateOfFire = 1.0f;
    [SerializeField] private float _startSpeedBullet = 1.0f;

    [SerializeField] private float _maxRotationZ;
    [SerializeField] private float _maxRotationW;

    [Header("LimitLevels")]
    [SerializeField] private int _maxLevelDamage;
    [SerializeField] private int _maxLevelRicochet;
    [SerializeField] private int _maxLevelRadiusExplosion;
    [SerializeField] private int _maxLevelDamageExplosion;

    private const float _distanceZ = 30f;
    private const float _levelCoefficient = 0.5f;

    private Camera _camera;
    private Transform _transform;
    private AudioSource _audioSource;

    private float _radiusExplosion = 0.5f;

    public int LevelDamage { get; private set; } = 1;
    public int LevelRicochet { get; private set; } = 1;
    public int LevelRadiusExplosion { get; private set; } = 1;
    public int LevelDamageExplosion { get; private set; } = 1;

    public event Action LevelLimitReachedDamage;
    public event Action LevelLimitReachedRicochet;
    public event Action LevelLimitReachedRadiusExplosion;
    public event Action LevelLimitReachedDamageExplosion;

    private void OnValidate()
    {
        if (_rateOfFire <= 0)
            _rateOfFire = 1.0f;

        if (_startSpeedBullet <= 0)
            _startSpeedBullet = 1.0f;
    }

    private void Awake()
    {
        _camera = Camera.main;
        _transform = transform;
        _audioSource = GetComponent<AudioSource>();
        _pool = new Pool<Bullet>(Preload, GetAction, ReturnAction);
    }

    public void LoadSave(int levelDamage, int levelRicochet, int levelDamageExplosion, int levelRadiusExplosion)
    {
        LevelDamage = levelDamage;
        LevelRicochet = levelRicochet;
        LevelDamageExplosion = levelDamageExplosion;
        LevelRadiusExplosion = levelRadiusExplosion;

        if (LevelDamage == _maxLevelDamage)
            LevelLimitReachedDamage?.Invoke();

        if (LevelRicochet == _maxLevelRicochet)
            LevelLimitReachedRicochet?.Invoke();

        if (LevelRadiusExplosion == _maxLevelRadiusExplosion)
            LevelLimitReachedRadiusExplosion?.Invoke();

        if (LevelDamageExplosion == _maxLevelDamageExplosion)
            LevelLimitReachedDamageExplosion?.Invoke();
    }

    public void Guidance(Vector2 mousePosition)
    {
        Vector3 mousePosition3D = mousePosition;
        mousePosition3D.z = _distanceZ;
        Vector3 worldMousePosition = _camera.ScreenToWorldPoint(mousePosition3D);
        worldMousePosition.z = _transform.position.z;

        Vector3 difference = worldMousePosition - _transform.position;
        difference.Normalize();

        float needAngle = Mathf.Atan2(difference.x, difference.y) * Mathf.Rad2Deg;
        _transform.rotation = Quaternion.Euler(0, 0, -needAngle);

        CheckGuidanceBoundaries();
    }

    public void Shoot()
    {
        if (YandexGame.isGamePlaying)
        {
            Bullet bullet = _pool.Get();
            bullet.SetStats(CalculateDamage(), LevelRicochet, CalculateRadiusExplosion(), CalculateDamageExplosion());
            bullet.transform.position = _spawnPoint.position;
            bullet.SetDirection(_transform.up);
            _particleSystem.Play();
            _audioSource.PlayOneShot(_audioSource.clip);
        }
    }

    public int UpLevelDamage()
    {
        LevelDamage++;

        if (LevelDamage == _maxLevelDamage)
            LevelLimitReachedDamage?.Invoke();

        return LevelDamage;
    }

    public int UpLevelRicochet()
    {
        LevelRicochet++;

        if (LevelRicochet == _maxLevelRicochet)
            LevelLimitReachedRicochet?.Invoke();

        return LevelRicochet;
    }

    public int UpLevelRadiusExplosion()
    {
        LevelRadiusExplosion++;

        if (LevelRadiusExplosion == _maxLevelRadiusExplosion)
            LevelLimitReachedRadiusExplosion?.Invoke();

        return LevelRadiusExplosion;
    }

    public int UpLevelDamageExplosion()
    {
        LevelDamageExplosion++;

        if (LevelDamageExplosion == _maxLevelDamageExplosion)
            LevelLimitReachedDamageExplosion?.Invoke();

        return LevelDamageExplosion;
    }

    public double CalculateCost()
    {
        double cost = _startCostShot + (_levelCoefficient * _startCostShot * (LevelDamage + LevelDamageExplosion + LevelRadiusExplosion));

        return cost;
    }

    private float CalculateDamage() => _damage + (_damage * LevelDamage);
    private float CalculateRadiusExplosion() => _radiusExplosion + (0.05f * LevelRadiusExplosion);
    private float CalculateDamageExplosion() => _damage * LevelDamageExplosion * 0.1f;

    private void CheckGuidanceBoundaries()
    {
        if (_transform.rotation.z > _maxRotationZ && _transform.rotation.w < _maxRotationW)
            _transform.rotation = new Quaternion(0, 0, _maxRotationZ, _maxRotationW);
        else if (_transform.rotation.z < -_maxRotationZ && _transform.rotation.w < _maxRotationW)
            _transform.rotation = new Quaternion(0, 0, -_maxRotationZ, _maxRotationW);
    }
    private Bullet Preload()
    {
        Bullet bullet = Instantiate(_prefab);
        bullet.SetPool(_pool);
        bullet.SetVelocity(_velocity);

        return bullet;
    }

    private void ReturnAction(Bullet bullet)
    {
        bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        bullet.gameObject.SetActive(false);
    }

    private void GetAction(Bullet bullet) => bullet.gameObject.SetActive(true);
}
