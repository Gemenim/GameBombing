using UnityEngine;
using YG;

public class Game : MonoBehaviour
{
    [SerializeField] Player _player;
    [SerializeField] private BombsGenerator _generator;
    [SerializeField] private CollectorCubes _collector;
    [SerializeField] private BarrierMover _barrierMover;
    [SerializeField] private ViewLevelBar _levelBar;
    [SerializeField] private TimerView _timerView;

    [Header("Windows")]
    [SerializeField] private HudScreen _hudScreen;
    [SerializeField] private SettingsScreen _settingsScreen;
    [SerializeField] private UpgrateScreen _upgrateScreen;
    [SerializeField] private LeaderbordScreen _leaderbordScreen;

    [SerializeField] private MuteSourceButton _muteSourceButton;

    private const float c_coefficientExperience = 0.4f;
    private const float c_levelCoefficientNeedExperience = 2.2f;
    private const float c_defoltNeedExperience = 1000f;

    private Bomb _bomb;
    private int _level = 1;

    public int Level => _level;

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
        _hudScreen.OnLeaderbordButtonClicked += OpenLeaderbord;

        _upgrateScreen.OnReturnButtonClicked += CloseUpgradeScreen;

        _settingsScreen.OnReturnButtonClicked += CloseSettingsScreen;
        _settingsScreen.OnResetSaveButtonClicked += ResetSeve;

        _leaderbordScreen.OnReturButtonClicked += CloseLeaderbord;

        _levelBar.OnButtonClicked += SpawnTsarBomb;
        _collector.PutCoins += GetExperience;
        _collector.ColectCore += SpawnNextBomb;

        YandexGame.onVisibilityWindowGame += _muteSourceButton.MuteWindow;
    }

    private void OnDisable()
    {
        _hudScreen.OnSaveButtonClicked -= SaveData;
        _hudScreen.OnUpgradeButtonClicked -= OpenUpgradeScreen;
        _hudScreen.OnSetingsButtonClicked -= OpenSettings;
        _hudScreen.OnLeaderbordButtonClicked -= OpenLeaderbord;

        _settingsScreen.OnReturnButtonClicked -= CloseSettingsScreen;
        _settingsScreen.OnResetSaveButtonClicked -= ResetSeve;

        _upgrateScreen.OnReturnButtonClicked -= CloseUpgradeScreen;

        _leaderbordScreen.OnReturButtonClicked -= CloseLeaderbord;

        _levelBar.OnButtonClicked -= SpawnTsarBomb;
        _collector.PutCoins -= GetExperience;
        _collector.ColectCore -= SpawnNextBomb;

        _bomb.Destroyed -= Spawn;

        YandexGame.onVisibilityWindowGame -= _muteSourceButton.MuteWindow;
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

    private void OpenLeaderbord()
    {
        YandexGame.GameplayStop();
        _leaderbordScreen.Open();
    }

    private void CloseLeaderbord()
    {
        YandexGame.GameplayStart();
        _leaderbordScreen.Close();
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
        _hudScreen.OffTimer();
        _level += 1;
        _levelBar.SetNeedExperience(GetNeedExperience(), _level);
        _barrierMover.Move();
        SaveData();
        UpdateTop();
        YandexGame.FullscreenShow();
    }

    private void GetExperience(float coins)
    {
        float experience = coins * c_coefficientExperience;
        _levelBar.AddExperience(experience);
    }

    private void Spawn(bool isTsarBomb)
    {
        _levelBar.OnDisableButton();

        if (isTsarBomb)
        {
            _bomb = _generator.Spawn(_level, isTsarBomb, _timerView);
            _hudScreen.OnTimer();
        }
        else
        {
            _bomb = _generator.Spawn(_level, isTsarBomb);
            _hudScreen.OffTimer();
        }

        _bomb.Destroyed += SpawnNextBomb;

    }

    private float GetNeedExperience()
    {
        float needExperience = c_defoltNeedExperience * Mathf.Pow(_level, c_levelCoefficientNeedExperience) - (c_defoltNeedExperience * _level);
        return needExperience > 0 ? needExperience : c_defoltNeedExperience;
    }

    private void SaveData()
    {
        YandexGame.savesData.LevelGame = _level;
        YandexGame.savesData.LevelUpgrade = _player.LevelUpgrade;
        YandexGame.savesData.LevelUpgradeDamage = _player.LevelUpgradeDamage;
        YandexGame.savesData.LevelUpgradeRicochet = _player.LevelUpgradeRicochet;
        YandexGame.savesData.LevelUpgradeDamageExplosion = _player.LevelUpgradeDamageExplosion;
        YandexGame.savesData.LevelUpgradeRadiusExplosion = _player.LevelUpgradeRadiusExplosion;
        YandexGame.savesData.CountDastroyBomb = _player.CountDasroyBombs;
        YandexGame.savesData.Coins = _player.Coins;
        YandexGame.savesData.Experience = _levelBar.Experience;
        UpdateTop();

        YandexGame.SaveProgress();
    }

    private void ResetSeve()
    {
        if (_bomb != null)
            Destroy(_bomb.gameObject);

        YandexGame.ResetSaveProgress();
        LoadSave();
        SaveData();
        Spawn(false);
    }

    private void LoadSave()
    {
        _level = YandexGame.savesData.LevelGame;
        _levelBar.SetNeedExperience(GetNeedExperience(), _level);
        _levelBar.AddExperience(YandexGame.savesData.Experience);
        _player.LoadSave(YandexGame.savesData.Coins, YandexGame.savesData.CountDastroyBomb, YandexGame.savesData.LevelUpgrade,
            YandexGame.savesData.LevelUpgradeDamage, YandexGame.savesData.LevelUpgradeRicochet,
            YandexGame.savesData.LevelUpgradeDamageExplosion, YandexGame.savesData.LevelUpgradeRadiusExplosion);
    }
}
