using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    public double Coins { get; private set; }

    public event Action<double> ChangeCount;
    public event Action Fail;

    public void LoadSave(double coins)
    {
        Coins = coins;
        ChangeCount?.Invoke(Coins);
    }

    public void PutCoins(double coins)
    {
        Coins += coins;
        ChangeCount?.Invoke(Coins);
    }

    public bool GetCoins(double requiredCoins)
    {
        if (Coins > requiredCoins)
        {
            Coins -= requiredCoins;
            ChangeCount?.Invoke(Coins);
            return true;
        }

        Fail?.Invoke();

        return false;
    }
}
