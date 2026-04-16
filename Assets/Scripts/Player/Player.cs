using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using YG;

public class Player : MonoBehaviour
{
    [SerializeField] private SavingHerald _savingHerald;
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

    [Header("Skills")]
    [SerializeField] private Damage _abilityDamage;
    [SerializeField] private Ricochet _abilityRicochet;
    [SerializeField] private SpeedExplosion _abilitySpeedExplosion;
    [SerializeField] private RadiusExposion _abilityRadiusExplosion;
    [SerializeField] private DamageExplosion _abilityDamageExplosion;

    private PlayerInput _input;
    private Camera _camera;
    private int _countDasroyBombs = 0;
    private Vector2 _positionMouse;

    public float Coins => _wallet.Coins;
    public int CountDasroyBombs => _countDasroyBombs;

    public bool IsBeginner { get; private set; } = true;

    private void Awake()
    {
        _input = new PlayerInput();
        _camera = Camera.main;

        _input.Player.Shoot.performed += OnShoot;
    }

    private void OnEnable()
    {
        _input.Enable();
        _savingHerald.Saved += Save;
        _savingHerald.Uploaded += Load;
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
        _savingHerald.Saved -= Save;
        _savingHerald.Uploaded -= Load;
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

    public PlayerInput GetInput() => _input;

    public void AddDastroyBomb()
    {
        _countDasroyBombs++;
    }

    private void Save()
    {
        YandexGame.savesData.CountDastroyBomb = _countDasroyBombs;
        YandexGame.savesData.Coins = _wallet.Coins;
    }

    private void Load()
    {
        _countDasroyBombs = YandexGame.savesData.CountDastroyBomb;
        _wallet.LoadSave(YandexGame.savesData.Coins);
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
            _abilityDamageExplosion.UpLevel();
    }

    private void UpRadiusExplosion(float cost)
    {
        if (_wallet.GetCoins(cost))
            _abilityRadiusExplosion.UpLevel();
    }

    private void UpRecochet(float cost)
    {
        if (_wallet.GetCoins(cost))
            _abilityRicochet.UpLevel();
    }

    private void UpDamage(float cost)
    {
        if (_wallet.GetCoins(cost))
            _abilityDamage.UpLevel();
    }

    private void UpSpeedAttack(float cost)
    {
        if (_wallet.GetCoins(cost))
            _gun.UpLevelSpeedAttack();
    }

    private void UpSpeedExplosion(float cost)
    {
        if (_wallet.GetCoins(cost))
            _abilitySpeedExplosion.UpLevel();
    }
}