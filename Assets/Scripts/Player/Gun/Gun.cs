using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Gun : MonoBehaviour
{
    private const float c_maxRotationZ = 0.66f;
    private const float c_maxRotationW = 0.74f;

    [SerializeField] private Pool<Bullet> _bulletPool;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Sky _sky;
    [SerializeField] private AudioSource _reckoscetSorce;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private UpdaterLevelUpgrad _updateLevelUpgrad;
    [SerializeField] private Slider _viewRecharge;

    [Header("Skills")]
    [SerializeField] private SpeedAttack _abilitySpeedAttack;

    private Transform _transform;
    private MoverGun _moverGun;
    private AudioSource _audioSource;

    public bool IsReadyShoot { get; private set; } = true;
    public float CostShot { get; private set; }

    private void Awake()
    {
        _transform = transform;
        _audioSource = GetComponent<AudioSource>();
        _bulletPool = new Pool<Bullet>(PreloadBullet, GetBulletAction, ReturnBulletAction);
    }

    public void Shoot()
    {
        Bullet bullet = _bulletPool.Get();
        bullet.SetStatus();
        //bullet.SetStats(_abilityDamage.AmountDamage, _abilityRicochet.Count, _abilityRadiusExplosion.Radius,
        //    _abilityDamageExplosion.CoefficientDamage, _abilitySpeedExplosion.DaleyExplosion);
        bullet.transform.SetParent(_sky.Bullets);
        bullet.transform.position = _spawnPoint.position;
        bullet.SetDirection(_transform.up);
        _particleSystem.Play();
        _audioSource.PlayOneShot(_audioSource.clip);

        if (_abilitySpeedAttack.TimeAttack <= 0)
            return;

        StartCoroutine(Recharge());
    }

    public void UpLevelSpeedAttack() => _abilitySpeedAttack.UpLevel();

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

        while (time < _abilitySpeedAttack.TimeAttack)
        {
            time += Time.deltaTime;
            _viewRecharge.value = time / _abilitySpeedAttack.TimeAttack;

            yield return null;
        }

        IsReadyShoot = true;
    }
}
