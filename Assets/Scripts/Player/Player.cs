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
    [SerializeField] private ViewButtonUpgrade _radiusExplosionButton;
    [SerializeField] private ViewButtonUpgrade _damageExplosionButton;

    private PlayerInput _input;

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

    private void UpDamage(double cost)
    {
        if (_wallet.GetCoins(cost))
            _damgeButton.ChangeText(_gun.UpLevelDamage());
    }
}
