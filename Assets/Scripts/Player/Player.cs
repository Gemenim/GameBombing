using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using YG;

public class Player : MonoBehaviour
{
    [SerializeField] private MoverCart _moverCart;
    [SerializeField] private MoverGun _moverGun;
    [SerializeField] private Gun _gun;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private UpdaterLevelSkills _updaterLevelSkills;

    [Header("ButtonUpgrade")]
    [SerializeField] private ViewButtonUpgrade _damgeButton;
    [SerializeField] private ViewButtonUpgrade _ricochetButton;
    [SerializeField] private ViewButtonUpgrade _speedAttackButton;
    [SerializeField] private ViewButtonUpgrade _speedExplosionButton;
    [SerializeField] private ViewButtonUpgrade _radiusExplosionButton;
    [SerializeField] private ViewButtonUpgrade _damageExplosionButton;

    private PlayerInput _input;
    private Camera _camera;
    private int _countDasroyBombs = 0;

    public float Coins => _wallet.Coins;
    public int CountDasroyBombs => _countDasroyBombs;
    public int LevelUpgrade => _updaterLevelSkills.Level;
    public int LevelDamge => _gun.LevelDamge;
    public int LevelRicochet => _gun.LevelRicochet;
    public int LevelSpeedAttac => _gun.LevelSpeedAttac;
    public int LevelDamgeExplosion => _gun.LevelDamgeExplosion;
    public int LevelRadiusExplosion => _gun.LevelRadiusExplosion;
    public int LevelSpeedExplosion => _gun.LevelSpeedExplosion;

    private Vector2 _positionMouse;

    private void Awake()
    {
        _input = new PlayerInput();
        _camera = Camera.main;

        _input.Player.Shoot.performed += OnShoot;
    }

    private void OnEnable()
    {
        _input.Enable();
        _damgeButton.OnButtonClicked += UpDamage;
        _ricochetButton.OnButtonClicked += UpRecochet;
        _speedAttackButton.OnButtonClicked += UpSpeedAttack;
        _speedExplosionButton.OnButtonClicked += UpSpeedExplosion;
        _radiusExplosionButton.OnButtonClicked += UpRadiusExplosion;
        _damageExplosionButton.OnButtonClicked += UpDamageExplsoion;
    }

    private void OnDisable()
    {
        _input.Disable();
        _damgeButton.OnButtonClicked -= UpDamage;
        _ricochetButton.OnButtonClicked -= UpRecochet;
        _speedAttackButton.OnButtonClicked -= UpSpeedAttack;
        _speedExplosionButton.OnButtonClicked -= UpSpeedExplosion;
        _radiusExplosionButton.OnButtonClicked -= UpRadiusExplosion;
        _damageExplosionButton.OnButtonClicked -= UpDamageExplsoion;
    }

    private void Update()
    {
        if (YandexGame.isGamePlaying)
        {
            if (_input.Player.Move.ReadValue<Vector2>() != new Vector2(0, 0))
            {
                _positionMouse = _input.Player.Move.ReadValue<Vector2>();
                _moverGun.Move(_moverCart.Move(_camera.ScreenToWorldPoint(_positionMouse).x));
            }
        }
    }

    public void AddDastroyBomb()
    {
        _countDasroyBombs++;
    }

    public void LoadSave(float coins, int countDasroyBombs, int damge, int ricochet, int speedAttac, int damgeExplosion, int radiusExplosion, int speedExplosion)
    {
        _countDasroyBombs = countDasroyBombs;
        _wallet.LoadSave(coins);        
        _gun.LoadSeve(damge, ricochet, speedAttac, damgeExplosion, radiusExplosion, speedExplosion);
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (YandexGame.isGamePlaying)
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                if (_gun.IsReadyShoot)
                {
                    if (_wallet.GetCoins(_updaterLevelSkills.Cost))
                    {
                        _gun.Shoot();
                    }
                }
            }
        }
    }

    private void UpDamageExplsoion(float cost)
    {
        if (_wallet.GetCoins(cost))
            _gun.UpLevelDamageExplosion();
    }

    private void UpRadiusExplosion(float cost)
    {
        if (_wallet.GetCoins(cost))
            _gun.UpLevelRadiusExplosion();
    }

    private void UpRecochet(float cost)
    {
        if (_wallet.GetCoins(cost))
            _gun.UpLevelRicochet();
    }

    private void UpDamage(float cost)
    {
        if (_wallet.GetCoins(cost))
            _gun.UpLevelDamage();
    }

    private void UpSpeedAttack(float cost)
    {
        if (_wallet.GetCoins(cost))
            _gun.UpLevelSpeedAttack();
    }

    private void UpSpeedExplosion(float cost)
    {
        if (_wallet.GetCoins(cost))
            _gun.UpLevelSpeedExplosion();
    }
}