using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Gun : MonoBehaviour
{
    private const float c_distanceZ = 30f;
    private const float c_levelCoefficientDamage = 1.9f;
    private const float c_levelCostCoefficient = 1.95f;
    private const float c_defoltRadiusExplosion = 0.1f;
    private const float c_defoltSpeedExplosion = 30f;
    private const float c_stepSpeedExplosion = 0.3f;
    private const float c_coefficientRadiusExplosion = 0.15f;
    private const float c_coefficientDamageExplosion = 0.02f;
    private const float c_coefficientRecharge = 0.04f;

    private const float c_maxRotationZ = 0.66f;
    private const float c_maxRotationW = 0.74f;

    [SerializeField] private Pool<Bullet> _bulletPool;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Sky _sky;
    [SerializeField] private AudioSource _reckoscetSorce;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private UpdateLevelUpgrad _updateLevelUpgrad;
    [SerializeField] private Slider _viewRecharge;

    [Header("Setings Gun")]
    [SerializeField] private float _velocity;
    [SerializeField] private float _defoltDamage = 1;
    [SerializeField] private float _startCostShot = 1;
    [SerializeField] private float _timeRecharge = 2.5f;


    [Header("Limit Levels")]
    [SerializeField] private int _maxLevelDamage;
    [SerializeField] private int _maxLevelRicochet;
    [SerializeField] private int _maxLevelSpeedAttack;
    [SerializeField] private int _maxLevelSpeedExplosion;
    [SerializeField] private int _maxLevelRadiusExplosion;
    [SerializeField] private int _maxLevelDamageExplosion;

    private Camera _camera;
    private Transform _transform;
    private AudioSource _audioSource;

    private float _damage;
    private float _radiusExplosion;
    private float _coefficientDamageExplosion;
    private float _daleyExplosion;

    public bool IsReadyShoot { get; private set; } = true;
    public float CostShot { get; private set; }
    public int LevelUpgrade { get; private set; } = 1;
    public int LevelDamage { get; private set; } = 1;
    public int LevelRicochet { get; private set; } = 1;
    public int LevelSeedAttack { get; private set; } = 1;
    public int LevelSpeedExplosion { get; private set; } = 1;
    public int LevelRadiusExplosion { get; private set; } = 1;
    public int LevelDamageExplosion { get; private set; } = 1;

    public event Action LevelLimitReachedDamage;
    public event Action LevelLimitReachedRicochet;
    public event Action LevelLimitReachedSpeedAttack;
    public event Action LevelLimitReachedSpeedExplosion;
    public event Action LevelLimitReachedRadiusExplosion;
    public event Action LevelLimitReachedDamageExplosion;

    private void OnValidate()
    {
        if (_velocity <= 0)
            _velocity = 20f;

        if (_defoltDamage <= 0)
            _defoltDamage = 1f;

        if (_startCostShot <= 0)
            _startCostShot = 0.5f;

        if (_maxLevelDamage <= 0)
            _maxLevelDamage = 100;

        if (_maxLevelDamageExplosion <= 0)
            _maxLevelDamageExplosion = 40;

        if (_maxLevelSpeedExplosion <= 0)
            _maxLevelSpeedExplosion = 20;

        if (_maxLevelRadiusExplosion <= 0)
            _maxLevelRadiusExplosion = 40;

        if (_maxLevelRicochet <= 0)
            _maxLevelRicochet = 50;

        if (_maxLevelSpeedAttack <= 0 || _maxLevelSpeedAttack > 50)
            _maxLevelSpeedAttack = 50;

        if (_timeRecharge <= 0)
            _timeRecharge = 1;
    }

    private void Awake()
    {
        _camera = Camera.main;
        _transform = transform;
        _audioSource = GetComponent<AudioSource>();
        _bulletPool = new Pool<Bullet>(PreloadBullet, GetBulletAction, ReturnBulletAction);
    }

    public void LoadSave(int levelUpgrade, int levelDamage, int levelRicochet, int levelSpeedAttack, int levelSpeedExplosion,int levelDamageExplosion, int levelRadiusExplosion)
    {
        LevelUpgrade = levelUpgrade;
        LevelDamage = levelDamage;
        LevelRicochet = levelRicochet;
        LevelSeedAttack = levelSpeedAttack;
        LevelSpeedExplosion = levelSpeedExplosion;
        LevelDamageExplosion = levelDamageExplosion;
        LevelRadiusExplosion = levelRadiusExplosion;

        _damage = LevelCalculator.Calculat(_defoltDamage, c_levelCoefficientDamage, LevelDamage);
        _damage = _damage > 0 ? _damage : _defoltDamage;
        _timeRecharge -= LevelSeedAttack * c_coefficientRecharge;
        _radiusExplosion = c_defoltRadiusExplosion + (c_coefficientRadiusExplosion * LevelRadiusExplosion);
        _coefficientDamageExplosion = LevelDamageExplosion * c_coefficientDamageExplosion;
        _daleyExplosion = c_defoltSpeedExplosion - c_stepSpeedExplosion * LevelSpeedExplosion;
        CalculateCost();

        if (LevelDamage == _maxLevelDamage)
            LevelLimitReachedDamage?.Invoke();

        if (LevelRicochet == _maxLevelRicochet)
            LevelLimitReachedRicochet?.Invoke();

        if (LevelSpeedExplosion == _maxLevelSpeedExplosion)
            LevelLimitReachedSpeedExplosion?.Invoke();

        if (LevelRadiusExplosion == _maxLevelRadiusExplosion)
            LevelLimitReachedRadiusExplosion?.Invoke();

        if (LevelDamageExplosion == _maxLevelDamageExplosion)
            LevelLimitReachedDamageExplosion?.Invoke();

        _updateLevelUpgrad.ChangeText(LevelUpgrade, CostShot);
    }

    public void Guidance(Vector2 mousePosition)
    {
        Vector3 mousePosition3D = mousePosition;
        mousePosition3D.z = c_distanceZ;
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
        Bullet bullet = _bulletPool.Get();
        bullet.SetStats(_damage, LevelRicochet, _radiusExplosion, _coefficientDamageExplosion, _daleyExplosion);
        bullet.transform.SetParent(_sky.Bullets);
        bullet.transform.position = _spawnPoint.position;
        bullet.SetDirection(_transform.up);
        _particleSystem.Play();
        _audioSource.PlayOneShot(_audioSource.clip);

        if (_timeRecharge <= 0)
            return;

        StartCoroutine(Recharge());
    }

    public int UpLevelDamage()
    {
        LevelDamage++;
        _damage = LevelCalculator.Calculat(_defoltDamage, c_levelCoefficientDamage, LevelDamage);

        if (_damage <= 0)
            _damage = _defoltDamage;

        UpLevelUpgrade();

        if (LevelDamage == _maxLevelDamage)
            LevelLimitReachedDamage?.Invoke();

        return LevelDamage;
    }

    public int UpLevelRicochet()
    {
        LevelRicochet++;
        UpLevelUpgrade();

        if (LevelRicochet == _maxLevelRicochet)
            LevelLimitReachedRicochet?.Invoke();

        return LevelRicochet;
    }

    public int UpLevelSpeedAttack()
    {
        LevelSeedAttack++;
        _timeRecharge -= c_coefficientRecharge;
        UpLevelUpgrade();

        if (LevelSeedAttack == _maxLevelSpeedAttack)
            LevelLimitReachedSpeedAttack?.Invoke();

        return LevelSeedAttack;
    }

    public int UpLevelRadiusExplosion()
    {
        LevelRadiusExplosion++;
        _radiusExplosion = c_defoltRadiusExplosion + (c_coefficientRadiusExplosion * LevelRadiusExplosion);
        UpLevelUpgrade();

        if (LevelRadiusExplosion == _maxLevelRadiusExplosion)
            LevelLimitReachedRadiusExplosion?.Invoke();

        return LevelRadiusExplosion;
    }

    public int UpLevelDamageExplosion()
    {
        LevelDamageExplosion++;
        _coefficientDamageExplosion = LevelDamageExplosion * c_coefficientDamageExplosion;
        UpLevelUpgrade();

        if (LevelDamageExplosion == _maxLevelDamageExplosion)
            LevelLimitReachedDamageExplosion?.Invoke();

        return LevelDamageExplosion;
    }

    public int UpLevelSpeedExplosion()
    {
        LevelSpeedExplosion++;
        _daleyExplosion = c_defoltSpeedExplosion - c_stepSpeedExplosion * LevelSpeedExplosion;
        UpLevelUpgrade();

        if (LevelSpeedExplosion == _maxLevelSpeedExplosion)
            LevelLimitReachedSpeedExplosion?.Invoke();

        return LevelSpeedExplosion;
    }

    private void CalculateCost()
    {
        CostShot = LevelCalculator.Calculat(_startCostShot, c_levelCostCoefficient, LevelUpgrade);
    }

    private void UpLevelUpgrade()
    {
        LevelUpgrade++;
        CalculateCost();
        _updateLevelUpgrad.ChangeText(LevelUpgrade, CostShot);
    }

    private void CheckGuidanceBoundaries()
    {
        if (_transform.rotation.z > c_maxRotationZ && _transform.rotation.w < c_maxRotationW)
            _transform.rotation = new Quaternion(0, 0, c_maxRotationZ, c_maxRotationW);
        else if (_transform.rotation.z < -c_maxRotationZ && _transform.rotation.w < c_maxRotationW)
            _transform.rotation = new Quaternion(0, 0, -c_maxRotationZ, c_maxRotationW);
    }

    private Bullet PreloadBullet()
    {
        Bullet bullet = Instantiate(_bulletPrefab);
        bullet.SetAudioSource(_reckoscetSorce);
        bullet.SetPool(_bulletPool);
        bullet.SetVelocity(_velocity);

        return bullet;
    }

    private void ReturnBulletAction(Bullet bullet)
    {
        bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        bullet.gameObject.SetActive(false);
    }

    private void GetBulletAction(Bullet bullet) => bullet.gameObject.SetActive(true);

    private IEnumerator Recharge()
    {
        IsReadyShoot = false;

        float time = 0;

        while (time < _timeRecharge)
        {
            time += Time.deltaTime;
            _viewRecharge.value = time / _timeRecharge;

            yield return null;
        }

        IsReadyShoot = true;
    }
}
