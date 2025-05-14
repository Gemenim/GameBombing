using UnityEngine;
using YG;

public class Game : MonoBehaviour
{
    [SerializeField] Player _player;
    [SerializeField] private BombsGenerator _generator;
    [SerializeField] private CollectorCubes _collector;
    [SerializeField] private BarrierMover _barrierMover;
    [SerializeField] private ViewBar _viewBar;

    [Header("Windows")]
    [SerializeField] private HudScreen _hudScreen;
    [SerializeField] private SettingsScreen _settingsScreen;
    [SerializeField] private UpgrateScreen _upgrateScreen;
    [SerializeField] private LiderbordScreen _liderbordScreen;

    private const float _levelCoefficientExperience = 0.5f;
    private const float _levelCoefficientNeedExperience = 1.5f;
    private const float _standartNeedExperience = 20f;

    private Bomb _bomb;
    private bool _onTsarBomb = false;
    private int _level = 1;

    public int Level => _level;

    private float _startNeedExperience => _standartNeedExperience * _level * _levelCoefficientNeedExperience;

    private void Start()
    {
        if (YandexGame.SDKEnabled)
            LoadSave();

        YandexGame.GameplayStart();
        _barrierMover.Move();
    }

    private void OnEnable()
    {
        _hudScreen.OnSaveButtonClicked += SaveData;
        _hudScreen.OnUpgradeButtonClicked += OpenUpgradeScreen;
        _hudScreen.OnSetingsButtonClicked += OpenSettings;
        _hudScreen.OnLiderbordButtonClicked += OpenLiderbordScreen;

        _upgrateScreen.OnReturnButtonClicked += CloseUpgradeScreen;

        _settingsScreen.OnReturnButtonClicked += CloseSettingsScreen;
        _settingsScreen.OnResetSaveButtonClicked += ResetSeve;

        _liderbordScreen.OnReturnButtonClicked += CloseLiderbordScreen;

        _viewBar.OnButtonClicked += SpawnTsarBomb;
        _collector.PutCoins += GetExperience;
        _collector.ColectCore += _player.AddDastroyBombs;
    }

    private void OnDisable()
    {
        _hudScreen.OnSaveButtonClicked -= SaveData;
        _hudScreen.OnUpgradeButtonClicked -= OpenUpgradeScreen;
        _hudScreen.OnSetingsButtonClicked -= OpenSettings;
        _hudScreen.OnLiderbordButtonClicked -= OpenLiderbordScreen;

        _settingsScreen.OnReturnButtonClicked -= CloseSettingsScreen;
        _settingsScreen.OnResetSaveButtonClicked -= ResetSeve;

        _upgrateScreen.OnReturnButtonClicked -= CloseUpgradeScreen;

        _liderbordScreen.OnReturnButtonClicked -= CloseLiderbordScreen;

        _viewBar.OnButtonClicked -= SpawnTsarBomb;
        _collector.PutCoins -= GetExperience;
        _collector.ColectCore -= _player.AddDastroyBombs;

        _bomb.Destroyed -= LevelUp;
    }

    private void Update()
    {
        if (_bomb == null)
        {
            _onTsarBomb = false;
            Spawn();
        }
    }

    private void OpenLiderbordScreen()
    {
        YandexGame.GameplayStop();
        _liderbordScreen.Open();
    }

    private void CloseLiderbordScreen()
    {
        YandexGame.GameplayStart();
        _liderbordScreen.Close();
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
        _onTsarBomb = true;
        Spawn();
        _bomb.Destroyed += LevelUp;
    }

    private void LevelUp(bool isTsarBomb)
    {
        if (isTsarBomb)
        {
            Debug.Log("Up");
            _onTsarBomb = false;
            _level += 1;
            _viewBar.SetNeedExperience(_startNeedExperience, _level);
            _barrierMover.Move();
            SaveData();
            YandexGame.FullscreenShow();
        }
    }

    private void GetExperience(double coins)
    {
        double experience = (coins * _levelCoefficientExperience) / 100;
        _viewBar.SetValue(experience);
    }

    private void Spawn()
    {
        _bomb = _generator.Spawn(_level, _onTsarBomb);
    }

    private void SaveData()
    {
        YandexGame.savesData.LevelGame = _level;
        YandexGame.savesData.LevelUpgadeDamage = _player.LevelUpgradeDamage;
        YandexGame.savesData.LevelUpgadeDamageExplosion = _player.LevelUpgradeDamageExplosion;
        YandexGame.savesData.LevelUpgadeRadiusExplosion = _player.LevelUpgradeRadiusExplosion;
        YandexGame.savesData.CountDastroyBomb = _player.CountDasroyBombs;
        YandexGame.savesData.Coins = _player.Coins;
        YandexGame.savesData.Experience = _viewBar.Experience;

        YandexGame.SaveProgress();
    }

    private void ResetSeve()
    {
        Destroy(_bomb.gameObject);
        YandexGame.ResetSaveProgress();
        LoadSave();
        Spawn();
    }

    private void LoadSave()
    {
        _level = YandexGame.savesData.LevelGame;
        _viewBar.SetNeedExperience(_startNeedExperience, _level);
        _viewBar.SetValue(YandexGame.savesData.Experience);
        _player.LoadSave(YandexGame.savesData.Coins, YandexGame.savesData.CountDastroyBomb, YandexGame.savesData.LevelUpgadeDamage, YandexGame.savesData.LevelUpgadeDamageExplosion, YandexGame.savesData.LevelUpgadeRadiusExplosion);
    }
}
