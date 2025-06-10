using System;
using UnityEngine;
using YG;

public class Game : MonoBehaviour
{
    [SerializeField] Player _player;
    [SerializeField] private BombsGenerator _generator;
    [SerializeField] private CollectorCubes _collector;
    [SerializeField] private BarrierMover _barrierMover;
    [SerializeField] private ViewLevelBar _levelBar;

    [Header("Windows")]
    [SerializeField] private HudScreen _hudScreen;
    [SerializeField] private SettingsScreen _settingsScreen;
    [SerializeField] private UpgrateScreen _upgrateScreen;

    private const float _levelCoefficientExperience = 0.5f;
    private const float _levelCoefficientNeedExperience = 1.5f;
    private const float _standartNeedExperience = 10f;

    private Bomb _bomb;
    private int _level = 1;

    public int Level => _level;

    private float _startNeedExperience => _standartNeedExperience * _level * _levelCoefficientNeedExperience;

    private void Start()
    {
        if (YandexGame.SDKEnabled)
            LoadSave();

        YandexGame.GameplayStart();
        Spawn(false);
        _barrierMover.Move();
    }

    private void OnEnable()
    {
        _hudScreen.OnSaveButtonClicked += SaveData;
        _hudScreen.OnUpgradeButtonClicked += OpenUpgradeScreen;
        _hudScreen.OnSetingsButtonClicked += OpenSettings;

        _upgrateScreen.OnReturnButtonClicked += CloseUpgradeScreen;

        _settingsScreen.OnReturnButtonClicked += CloseSettingsScreen;
        _settingsScreen.OnResetSaveButtonClicked += ResetSeve;

        _levelBar.OnButtonClicked += SpawnTsarBomb;
        _collector.PutCoins += GetExperience;
        _collector.ColectCore += SpawnNextBomb;
    }

    private void OnDisable()
    {
        _hudScreen.OnSaveButtonClicked -= SaveData;
        _hudScreen.OnUpgradeButtonClicked -= OpenUpgradeScreen;
        _hudScreen.OnSetingsButtonClicked -= OpenSettings;

        _settingsScreen.OnReturnButtonClicked -= CloseSettingsScreen;
        _settingsScreen.OnResetSaveButtonClicked -= ResetSeve;

        _upgrateScreen.OnReturnButtonClicked -= CloseUpgradeScreen;

        _levelBar.OnButtonClicked -= SpawnTsarBomb;
        _collector.PutCoins -= GetExperience;
        _collector.ColectCore -= SpawnNextBomb;

        _bomb.Destroyed -= Spawn;
    }

    private void OpenUpgradeScreen()
    {
        YandexGame.GameplayStop();
        _upgrateScreen.Open();
    }

    private void CloseUpgradeScreen()
    {
        YandexGame.GameplayStart();
        _upgrateScreen.Close();
    }

    private void OpenSettings()
    {
        YandexGame.GameplayStop();
        _settingsScreen.Open();
    }

    private void CloseSettingsScreen()
    {
        YandexGame.GameplayStart();
        _settingsScreen.Close();
    }

    [ContextMenu("Tsar")]
    private void SpawnTsarBomb()
    {
        Destroy(_bomb.gameObject);
        Spawn(true);
    }

    private void SpawnNextBomb(bool isTsarBomb)
    {
        if (isTsarBomb)
            LevelUp();

        _player.AddDastroyBomb();
        Spawn(false);
    }

    private void UpdateTop()
    {
        YandexGame.NewLeaderboardScores("Level", Level);
        YandexGame.NewLeaderboardScores("Coins", (long)_player.Coins);
        YandexGame.NewLeaderboardScores("DestroyedBombs", Level);
    }

    private void LevelUp()
    {
        Debug.Log("Up");
        _level += 1;
        _levelBar.SetNeedExperience(_startNeedExperience, _level);
        _barrierMover.Move();
        SaveData();
        UpdateTop();
        YandexGame.FullscreenShow();
    }

    private void GetExperience(double coins)
    {
        double experience = (coins * _levelCoefficientExperience) / 100;
        _levelBar.SetValue(experience);
    }

    private void Spawn(bool isTsarBomb)
    {
        _levelBar.OnDisableButton();
        _bomb = _generator.Spawn(_level, isTsarBomb);
        _bomb.Destroyed += SpawnNextBomb;
    }

    private void SaveData()
    {
        YandexGame.savesData.LevelGame = _level;
        YandexGame.savesData.LevelUpgadeDamage = _player.LevelUpgradeDamage;
        YandexGame.savesData.LevelUpgadeRicochet = _player.LevelUpgradeRicochet;
        YandexGame.savesData.LevelUpgadeDamageExplosion = _player.LevelUpgradeDamageExplosion;
        YandexGame.savesData.LevelUpgadeRadiusExplosion = _player.LevelUpgradeRadiusExplosion;
        YandexGame.savesData.CountDastroyBomb = _player.CountDasroyBombs;
        YandexGame.savesData.Coins = _player.Coins;
        YandexGame.savesData.Experience = _levelBar.Experience;
        UpdateTop();

        YandexGame.SaveProgress();
    }

    private void ResetSeve()
    {
        Destroy(_bomb.gameObject);
        YandexGame.ResetSaveProgress();
        LoadSave();
        Spawn(false);
    }

    private void LoadSave()
    {
        _level = YandexGame.savesData.LevelGame;
        _levelBar.SetNeedExperience(_startNeedExperience, _level);
        _levelBar.SetValue(YandexGame.savesData.Experience);
        _player.LoadSave(YandexGame.savesData.Coins, YandexGame.savesData.CountDastroyBomb, YandexGame.savesData.LevelUpgadeDamage, YandexGame.savesData.LevelUpgadeRicochet, YandexGame.savesData.LevelUpgadeDamageExplosion, YandexGame.savesData.LevelUpgadeRadiusExplosion);
    }
}
