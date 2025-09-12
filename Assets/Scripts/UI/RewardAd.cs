using UnityEngine;
using YG;

public class RewardAd : MonoBehaviour
{
    [SerializeField] private int AdID;
    [SerializeField] private Game _game;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private Gun _gun;
    [SerializeField] private float _countAddCoins = 300f;

    private const float c_levelCoefficient = 2f;
    private const int c_countShot = 5;

    private void OnValidate()
    {
        if (_countAddCoins <= 0)
            _countAddCoins = 500f;
    }

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
        float count = (_countAddCoins * (Mathf.Pow(_game.Level, c_levelCoefficient)) - (_countAddCoins * _game.Level)) + (_gun.CostShot * c_countShot);
        count = count <= 0 ? _countAddCoins : count;
        _wallet.PutCoins(count);
    }
}
