using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    public float Coins { get; private set; }

    public event Action<float> ChangeCount;
    public event Action Fail;

    public void LoadSave(float coins)
    {
        Coins = coins;
        ChangeCount?.Invoke(Coins);
    }

    public void PutCoins(float coins)
    {
        Coins += coins;
        ChangeCount?.Invoke(Coins);
    }

    public bool GetCoins(float requiredCoins)
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
