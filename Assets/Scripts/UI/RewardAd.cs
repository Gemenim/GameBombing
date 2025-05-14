using UnityEngine;
using YG;

public class RewardAd : MonoBehaviour
{
    [SerializeField] private int AdID;
    [SerializeField] private Game _game;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private double _countAddCoins = 100;

    private float _levelCoefficient = 1.5f;

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += Reward;
    }
    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= Reward;
    }

    private void Reward(int id)
    {
        if (id == AdID)
            AddCoins();
    }

    void AddCoins()
    {
        double count = _countAddCoins * _game.Level * _levelCoefficient;
        _wallet.PutCoins(count);
    }
}
