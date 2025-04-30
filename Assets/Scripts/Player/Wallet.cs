using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private double _coins = 0;

    public event Action<double> ChangeCount;
    public event Action Fail;

    private void Start()
    {
        ChangeCount?.Invoke(_coins);
    }

    public void PutCoins(double coins)
    {
        _coins += coins;
        ChangeCount?.Invoke(_coins);
    }

    public bool GetCoins(double requiredCoins)
    {
        if (_coins > requiredCoins)
        {
            _coins -= requiredCoins;
            ChangeCount?.Invoke(_coins);
            return true;
        }

        Fail?.Invoke();

        return false;
    }
}
