using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private MoverCart _cart;
    [SerializeField] private Gun _gun;
    [SerializeField] private Wallet _wallet;

    private PlayerInput _input;

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
        _gun.Guidance(_input.Player.Guidance.ReadValue<Vector2>());
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        _gun.Shoot();
        _cart.MoveCar();
    }
}
