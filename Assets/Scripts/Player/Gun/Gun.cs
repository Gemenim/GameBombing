using System;
using System.Collections;
using UnityEngine;
using YG;

[RequireComponent(typeof(AudioSource))]
public class Gun : MonoBehaviour
{
    [SerializeField] private Pool<Bullet> _pool;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Bullet _prefab;
    [SerializeField] private AudioSource _reckoscetSorce;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private UpdateLevelUpgrad _updateLevelUpgrad;
    [SerializeField] private float _velocity;
    [SerializeField] private float _defoltDamage = 1;
    [SerializeField] private float _startCostShot = 1;
    [SerializeField] private float _timeRecharge = 2.5f;


    [Header("Limit Levels")]
    [SerializeField] private int _maxLevelDamage;
    [SerializeField] private int _maxLevelRicochet;
    [SerializeField] private int _maxLevelSpeedAttack;
    [SerializeField] private int _maxLevelRadiusExplosion;
    [SerializeField] private int _maxLevelDamageExplosion;

    private const float c_distanceZ = 30f;
    private const float c_levelCoefficientDamage = 2f;
    private const float c_levelCostCoefficient = 2.25f;
    private const float c_defoltRadiusExplosion = 0.5f;
    private const float c_coefficientRadiusExplosion = 0.01f;
    private const float c_coefficientDamageExplosion = 0.2f;
    private const float c_coefficientRecharge = 0.04f;

    private const float c_maxRotationZ = 0.66f;
    private const float c_maxRotationW = 0.74f;

    private Camera _camera;
    private Transform _transform;
    private AudioSource _audioSource;

    private bool _isReadyShoot = true;
    private float _damage;
    private float _radiusExplosion;
    private float _damageExplosion;

    public float CostShot { get; private set; }
    public int LevelUpgrade { get; private set; } = 1;
    public int LevelDamage { get; private set; } = 1;
    public int LevelRicochet { get; private set; } = 1;
    public int LevelSeedAttack { get; private set; } = 1;
    public int LevelRadiusExplosion { get; private set; } = 1;
    public int LevelDamageExplosion { get; private set; } = 1;

    public event Action LevelLimitReachedDamage;
    public event Action LevelLimitReachedRicochet;
    public event Action LevelLimitReachedSeedAttack;
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
        _pool = new Pool<Bullet>(Preload, GetAction, ReturnAction);
    }

    public void LoadSave(int levelUpgrade, int levelDamage, int levelRicochet, int levelSpeedAttack, int levelDamageExplosion, int levelRadiusExplosion)
    {
        LevelUpgrade = levelUpgrade;
        LevelDamage = levelDamage;
        LevelRicochet = levelRicochet;
        LevelSeedAttack = levelSpeedAttack;
        LevelDamageExplosion = levelDamageExplosion;
        LevelRadiusExplosion = levelRadiusExplosion;

        _damage = _defoltDamage * Mathf.Pow(LevelDamage, c_levelCoefficientDamage) - (_defoltDamage * LevelDamage);
        _damage = _damage > 0 ? _damage : _defoltDamage;
        _timeRecharge -= LevelSeedAttack * c_coefficientRecharge;
        _radiusExplosion = c_defoltRadiusExplosion + (c_coefficientRadiusExplosion * LevelRadiusExplosion);
        _damageExplosion = _defoltDamage * LevelDamageExplosion * c_coefficientDamageExplosion;
        CalculateCost();

        if (LevelDamage == _maxLevelDamage)
            LevelLimitReachedDamage?.Invoke();

        if (LevelRicochet == _maxLevelRicochet)
            LevelLimitReachedRicochet?.Invoke();

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
        if (YandexGame.isGamePlaying)
        {
            if (_isReadyShoot)
            {
                Bullet bullet = _pool.Get();
                bullet.SetStats(_damage, LevelRicochet, _radiusExplosion, _damageExplosion);
                bullet.transform.position = _spawnPoint.position;
                bullet.SetDirection(_transform.up);
                _particleSystem.Play();
                _audioSource.PlayOneShot(_audioSource.clip);

                if (_timeRecharge <= 0)
                    return;

                StartCoroutine(Recharge());
            }
        }
    }

    public int UpLevelDamage()
    {
        LevelDamage++;
        _damage = _defoltDamage * Mathf.Pow(LevelDamage, c_levelCoefficientDamage) - (_defoltDamage * LevelDamage);

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
            LevelLimitReachedSeedAttack?.Invoke();

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
        _damageExplosion = _defoltDamage * LevelDamageExplosion * c_coefficientDamageExplosion;
        UpLevelUpgrade();

        if (LevelDamageExplosion == _maxLevelDamageExplosion)
            LevelLimitReachedDamageExplosion?.Invoke();

        return LevelDamageExplosion;
    }

    private void CalculateCost()
    {
        CostShot = _startCostShot * (Mathf.Pow(LevelUpgrade, c_levelCostCoefficient)) - (_startCostShot * LevelUpgrade);
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

    private Bullet Preload()
    {
        Bullet bullet = Instantiate(_prefab);
        bullet.SetAudioSource(_reckoscetSorce);
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

    private IEnumerator Recharge()
    {
        _isReadyShoot = false;
        yield return new WaitForSeconds(_timeRecharge);
        _isReadyShoot = true;
    }
}
