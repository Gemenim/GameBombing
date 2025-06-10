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
    [SerializeField] private ViewButtonUpgrade _radiusExplosionButton;
    [SerializeField] private ViewButtonUpgrade _damageExplosionButton;

    private PlayerInput _input;
    private int _countDasroyBombs = 0;

    public double Coins => _wallet.Coins;
    public int CountDasroyBombs => _countDasroyBombs;
    public int LevelUpgradeDamage => _gun.LevelDamage;
    public int LevelUpgradeRicochet => _gun.LevelRicochet;
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
        _radiusExplosionButton.OnButtonClicked += UpRadiusExplosion;
        _damageExplosionButton.OnButtonClicked += UpDamageExplsoion;
        _gun.LevelLimitReachedDamage += OnDisableButtonDamage;
        _gun.LevelLimitReachedRadiusExplosion += OnDisableButtonRadiusExplosion;
        _gun.LevelLimitReachedDamageExplosion += OnDisableButtonDamageExplosion;
    }

    private void OnDisable()
    {
        _input.Disable();
        _damgeButton.OnButtonClicked -= UpDamage;
        _ricochetButton.OnButtonClicked -= UpRecochet;
        _radiusExplosionButton.OnButtonClicked -= UpRadiusExplosion;
        _damageExplosionButton.OnButtonClicked -= UpDamageExplsoion;
        _gun.LevelLimitReachedDamage -= OnDisableButtonDamage;
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

    public void LoadSave(double coins, int countDasroyBombs, int levelDamage, int levelRicochet, int levelDamageExplosion, int levelRadiusExplosion)
    {
        _wallet.LoadSave(coins);
        _gun.LoadSave(levelDamage, levelRicochet, levelDamageExplosion, levelRadiusExplosion);
        _damageExplosionButton.ChangeText(_gun.LevelDamageExplosion);
        _radiusExplosionButton.ChangeText(_gun.LevelRadiusExplosion);
        _ricochetButton.ChangeText(_gun.LevelRicochet);
        _damgeButton.ChangeText(_gun.LevelDamage);
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (YandexGame.isGamePlaying)
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                if (_wallet.GetCoins(_gun.CalculateCost()))
                {
                    _gun.Shoot();
                    _cart.MoveCar();
                }
            }
        }
    }

    private void OnDisableButtonDamage() => _damgeButton.enabled = false;
    private void OnDisableButtonRadiusExplosion() => _radiusExplosionButton.enabled = false;
    private void OnDisableButtonDamageExplosion() => _damageExplosionButton.enabled = false;

    private void UpDamageExplsoion(double cost)
    {
        if (_wallet.GetCoins(cost))
            _damageExplosionButton.ChangeText(_gun.UpLevelDamageExplosion());
    }

    private void UpRadiusExplosion(double cost)
    {
        if (_wallet.GetCoins(cost))
            _radiusExplosionButton.ChangeText(_gun.UpLevelRadiusExplosion());
    }

    private void UpRecochet(double cost)
    {
        if (_wallet.GetCoins(cost))
            _ricochetButton.ChangeText(_gun.UpLevelRicochet());
    }

    private void UpDamage(double cost)
    {
        if (_wallet.GetCoins(cost))
            _damgeButton.ChangeText(_gun.UpLevelDamage());
    }
}
