using System;
using UnityEngine;
using YG;

public class Gun : MonoBehaviour
{
    [SerializeField] private Pool<Bullet> _pool;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Bullet _prefab;
    [SerializeField] private float _velocity;
    [SerializeField] private float _damage;
    [SerializeField] private double _startCostShot = 1;

    [SerializeField] private float _rateOfFire = 1.0f;
    [SerializeField] private float _startSpeedBullet = 1.0f;

    [SerializeField] private float _maxRotationZ;
    [SerializeField] private float _maxRotationW;

    [Header("LimitLevels")]
    [SerializeField] private int _maxLevelDamage;
    [SerializeField] private int _maxLevelRadiusExplosion;
    [SerializeField] private int _maxLevelDamageExplosion;

    private const float _distanceZ = 30f;
    private const float _levelCoefficient = 0.5f;

    private Camera _camera;
    private Transform _transform;

    private float _radiusExplosion = 0.5f;

    private int _levelDamage = 1;
    private int _levelRadiusExplosion = 1;
    private int _levelDamageExplosion = 1;

    public int LevelDamage => _levelDamage;
    public int LevelRadiusExplosion => _levelRadiusExplosion;
    public int LevelDamageExplosion => _levelDamageExplosion;

    public event Action LevelLimitReachedDamage;
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
        _pool = new Pool<Bullet>(Preload, GetAction, ReturnAction);
    }

    public void LoadSave(int levelDamage, int levelDamageExplosion, int levelRadiusExplosion)
    {
        _levelDamage = levelDamage;
        _levelDamageExplosion = levelDamageExplosion;
        _levelRadiusExplosion = levelRadiusExplosion;
        LevelLimitReachedDamage?.Invoke();
        LevelLimitReachedRadiusExplosion?.Invoke();
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
            bullet.SetStats(GetDamage(), GetRadiusExplosion(), GetDamageExplosion());
            bullet.transform.position = _spawnPoint.position;
            bullet.SetDirection(_transform.up);
        }
    }

    public int UpLevelDamage()
    {
        _levelDamage += 1;

        if (_levelDamage == _maxLevelDamage)
            LevelLimitReachedDamage?.Invoke();

        return _levelDamage;
    }

    public int UpLevelRadiusExplosion()
    {
        _levelRadiusExplosion += 1;

        if (_levelRadiusExplosion == _maxLevelRadiusExplosion)
            LevelLimitReachedRadiusExplosion?.Invoke();

        return _levelRadiusExplosion;
    }

    public int UpLevelDamageExplosion()
    {
        _levelDamageExplosion += 1;

        if (_levelDamageExplosion == _maxLevelDamageExplosion)
            LevelLimitReachedDamageExplosion?.Invoke();

        return _levelDamageExplosion;
    }
    public double CalculateCost()
    {
        double cost = _startCostShot + (_levelCoefficient * _startCostShot * (_levelDamage + _levelDamageExplosion + _levelRadiusExplosion));

        return cost;
    }

    private float GetDamage() => _damage + (_damage * _levelDamage);
    private float GetRadiusExplosion() => _radiusExplosion + (0.1f * _levelRadiusExplosion);
    private float GetDamageExplosion() => _damage * _levelDamageExplosion * 0.1f;

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
