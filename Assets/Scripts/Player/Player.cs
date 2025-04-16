using UnityEngine;
using UnityEngine.InputSystem;
using YG;

public class Player : MonoBehaviour
{
    [SerializeField] private MoverCart _cart;
    [SerializeField] private Gun _gun;
    [SerializeField] private Wallet _wallet;

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
    }

    private void OnDisable()
    {
        _input.Disable();
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
            _gun.Shoot();
            _cart.MoveCar();
        }
    }
}
