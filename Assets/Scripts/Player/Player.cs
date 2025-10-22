using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using YG;

public class Player : MonoBehaviour
{
    [SerializeField] private MoverCart _cart;
    [SerializeField] private Gun _gun;
    [SerializeField] private Wallet _wallet;

    [Header("ButtonUpgrade")]
    [SerializeField] private ViewButtonUpgrade _damgeButton;
    [SerializeField] private ViewButtonUpgrade _ricochetButton;
    [SerializeField] private ViewButtonUpgrade _speedAttackButton;
    [SerializeField] private ViewButtonUpgrade _speedExplosionButton;
    [SerializeField] private ViewButtonUpgrade _radiusExplosionButton;
    [SerializeField] private ViewButtonUpgrade _damageExplosionButton;

    private PlayerInput _input;
    private int _countDasroyBombs = 0;

    public float Coins => _wallet.Coins;
    public int CountDasroyBombs => _countDasroyBombs;
    public int LevelUpgrade => _gun.LevelUpgrade;
    public int LevelUpgradeDamage => _gun.LevelDamage;
    public int LevelUpgradeRicochet => _gun.LevelRicochet;
    public int LevelUpgradeSpeedAttack => _gun.LevelSeedAttack;
    public int LevelUpgradeSpeedExplosion => _gun.LevelSpeedExplosion;
    public int LevelUpgradeRadiusExplosion => _gun.LevelRadiusExplosion;
    public int LevelUpgradeDamageExplosion => _gun.LevelDamageExplosion;

    private Vector2 _positionMouse;

    private void Awake()
    {
        _input = new PlayerInput();

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
        _gun.LevelLimitReachedDamage += OnDisableButtonDamage;
        _gun.LevelLimitReachedRicochet += OnDisableButtonRecochet;
        _gun.LevelLimitReachedSpeedAttack += OnDisableButtonSpeedAttack;
        _gun.LevelLimitReachedSpeedExplosion += OnDisableButtonSpeedExplosion;
        _gun.LevelLimitReachedRadiusExplosion += OnDisableButtonRadiusExplosion;
        _gun.LevelLimitReachedDamageExplosion += OnDisableButtonDamageExplosion;
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
        _gun.LevelLimitReachedDamage -= OnDisableButtonDamage;
        _gun.LevelLimitReachedRicochet -= OnDisableButtonRecochet;
        _gun.LevelLimitReachedSpeedAttack -= OnDisableButtonSpeedAttack;
        _gun.LevelLimitReachedSpeedExplosion -= OnDisableButtonSpeedExplosion;
        _gun.LevelLimitReachedRadiusExplosion -= OnDisableButtonRadiusExplosion;
        _gun.LevelLimitReachedDamageExplosion -= OnDisableButtonDamageExplosion;
    }

    private void Update()
    {
        if (YandexGame.isGamePlaying)
        {
            if (_input.Player.Guidance.ReadValue<Vector2>() != new Vector2(0, 0))
            {
                _positionMouse = _input.Player.Guidance.ReadValue<Vector2>();
                _gun.Guidance(_positionMouse);
            }
        }
    }

    public void AddDastroyBomb()
    {
        _countDasroyBombs++;
    }

    public void LoadSave(float coins, int countDasroyBombs, int levelUpgrade, int levelDamage, int levelRicochet, int levelSpeedAttack, int levelSpeedExplosion, int levelDamageExplosion, int levelRadiusExplosion)
    {
        _countDasroyBombs = countDasroyBombs;
        _wallet.LoadSave(coins);
        _gun.LoadSave(levelUpgrade, levelDamage, levelRicochet, levelSpeedAttack, levelSpeedExplosion, levelDamageExplosion, levelRadiusExplosion);
        _damageExplosionButton.ChangeText(_gun.LevelDamageExplosion);
        _radiusExplosionButton.ChangeText(_gun.LevelRadiusExplosion);
        _ricochetButton.ChangeText(_gun.LevelRicochet);
        _damgeButton.ChangeText(_gun.LevelDamage);
        _speedAttackButton.ChangeText(_gun.LevelSeedAttack);
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (YandexGame.isGamePlaying)
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                if (_gun.IsReadyShoot)
                {
                    if (_wallet.GetCoins(_gun.CostShot))
                    {
                        _gun.Shoot();
                        _cart.MoveCar();
                    }
                }
            }
        }
    }

    private void OnDisableButtonDamage() => _damgeButton.enabled = false;
    private void OnDisableButtonRecochet() => _ricochetButton.enabled = false;
    private void OnDisableButtonSpeedAttack() => _speedAttackButton.enabled = false;
    private void OnDisableButtonSpeedExplosion() => _speedExplosionButton.enabled = false;
    private void OnDisableButtonRadiusExplosion() => _radiusExplosionButton.enabled = false;
    private void OnDisableButtonDamageExplosion() => _damageExplosionButton.enabled = false;

    private void UpDamageExplsoion(float cost)
    {
        if (_wallet.GetCoins(cost))
            _damageExplosionButton.ChangeText(_gun.UpLevelDamageExplosion());
    }

    private void UpRadiusExplosion(float cost)
    {
        if (_wallet.GetCoins(cost))
            _radiusExplosionButton.ChangeText(_gun.UpLevelRadiusExplosion());
    }

    private void UpRecochet(float cost)
    {
        if (_wallet.GetCoins(cost))
            _ricochetButton.ChangeText(_gun.UpLevelRicochet());
    }

    private void UpDamage(float cost)
    {
        if (_wallet.GetCoins(cost))
            _damgeButton.ChangeText(_gun.UpLevelDamage());
    }

    private void UpSpeedAttack(float cost)
    {
        if (_wallet.GetCoins(cost))
            _speedAttackButton.ChangeText(_gun.UpLevelSpeedAttack());
    }

    private void UpSpeedExplosion(float cost)
    {
        if (_wallet.GetCoins(cost))
            _speedExplosionButton.ChangeText(_gun.UpLevelSpeedExplosion());
    }
}