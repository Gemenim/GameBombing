using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    private double _coins = 0;

    public event Action<double> ChangeCount;

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

        return false;
    }
}
