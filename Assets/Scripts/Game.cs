using System.Collections;
using UnityEngine;
using YG;

public class Game : MonoBehaviour
{
    [SerializeField] Player _player;
    [SerializeField] private BombsGenerator _generator;
    [SerializeField] private Transform _bombs;
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

    [SerializeField] private string[] _nameLeaderbords;

    private const float c_coefficientExperience = 0.45f;
    private const float c_ñoefficientNeedExperience = 0.01f;
    private const int c_multiplierLevel = 10;
    private const float c_defoltNeedExperience = 1500f;

    private Bomb _bomb;
    private int _maxNumberAttempts = 5;
    private Coroutine _updateTop = null;
    private float _levelCoefficientNeedExperience = 2.435f;
    private float _dalayUpdateTop = 1f;

    public int Level { get; private set; } = 1;

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

        if (_bomb != null)
            _bomb.Dastroy -= RespawnBomb;

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
        StartUpdateTop();
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
        ClearOfBombs();
        Spawn(true);
    }

    private void SpawnNextBomb(bool isTsarBomb)
    {
        if (isTsarBomb)
            LevelUp();

        _player.AddDastroyBomb();
        Spawn(false);
    }

    private void RespawnBomb()
    {
        ClearOfBombs();
        _levelBar.SetNeedExperience(GetNeedExperience() / _maxNumberAttempts, Level);

        if (_maxNumberAttempts > 0)
            _maxNumberAttempts--;

        Spawn(false);
    }

    private void StartUpdateTop()
    {
        long[] score = new long[3] { (long)Level, (long)_player.Coins, (long)_player.CountDasroyBombs };

        if (_updateTop == null)
        {
            _updateTop = StartCoroutine(UpdateTop(score));
        }
        else
        {
            StopCoroutine(_updateTop);
            _updateTop = StartCoroutine(UpdateTop(score));
        }
    }

    private void LevelUp()
    {
        _hudScreen.OffTimer();
        Level += 1;
        _maxNumberAttempts = 5;
        GetCoefficientNeedExperience();
        _levelBar.SetNeedExperience(GetNeedExperience(), Level);
        _barrierMover.Move();
        SaveData();
        StartUpdateTop();
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
            _bomb = _generator.Spawn(Level, isTsarBomb, _timerView);
            _bomb.transform.SetParent(_bombs);
            _bomb.Dastroy += RespawnBomb;
            _hudScreen.OnTimer();
        }
        else
        {
            _bomb = _generator.Spawn(Level, isTsarBomb);
            _bomb.transform.SetParent(_bombs);
            _hudScreen.OffTimer();
        }
    }

    private float GetNeedExperience()
    {
        //float needExperience = c_defoltNeedExperience * Mathf.Pow(Level, _levelCoefficientNeedExperience) - (c_defoltNeedExperience * Level);
        float needExperience = LevelCalculator.Calculat(c_defoltNeedExperience, _levelCoefficientNeedExperience, Level, c_multiplierLevel, c_ñoefficientNeedExperience);
        return needExperience > 0 ? needExperience : c_defoltNeedExperience;
    }

    private void GetCoefficientNeedExperience()
    {
        int multiplier = Level / c_multiplierLevel;
        _levelCoefficientNeedExperience += c_ñoefficientNeedExperience * multiplier;
    }

    private void SaveData()
    {
        YandexGame.savesData.LevelGame = Level;
        YandexGame.savesData.LevelUpgrade = _player.LevelUpgrade;
        YandexGame.savesData.LevelUpgradeDamage = _player.LevelUpgradeDamage;
        YandexGame.savesData.LevelUpgradeRicochet = _player.LevelUpgradeRicochet;
        YandexGame.savesData.LevelUpgradeSpeedAttack = _player.LevelUpgradeSeedAttack;
        YandexGame.savesData.LevelUpgradeDamageExplosion = _player.LevelUpgradeDamageExplosion;
        YandexGame.savesData.LevelUpgradeRadiusExplosion = _player.LevelUpgradeRadiusExplosion;
        YandexGame.savesData.CountDastroyBomb = _player.CountDasroyBombs;
        YandexGame.savesData.Coins = _player.Coins;
        YandexGame.savesData.Experience = _levelBar.Experience;
        StartUpdateTop();

        YandexGame.SaveProgress();
    }

    private void ResetSeve()
    {
        ClearOfBombs();

        YandexGame.ResetSaveProgress();
        LoadSave();
        SaveData();
        Spawn(false);
    }

    private void LoadSave()
    {
        Level = YandexGame.savesData.LevelGame;
        _levelBar.AddExperience(YandexGame.savesData.Experience);
        _player.LoadSave(YandexGame.savesData.Coins, YandexGame.savesData.CountDastroyBomb, YandexGame.savesData.LevelUpgrade,
            YandexGame.savesData.LevelUpgradeDamage, YandexGame.savesData.LevelUpgradeRicochet, YandexGame.savesData.LevelUpgradeSpeedAttack,
            YandexGame.savesData.LevelUpgradeDamageExplosion, YandexGame.savesData.LevelUpgradeRadiusExplosion);
        GetCoefficientNeedExperience();
        _levelBar.SetNeedExperience(GetNeedExperience(), Level);
        StartUpdateTop();
    }

    private void ClearOfBombs()
    {
        int countObjects = _bombs.childCount;

        for (int i = 0; i < countObjects; i++)
            Destroy(_bombs.GetChild(i).gameObject);
    }

    private IEnumerator UpdateTop(long[] score)
    {
        WaitForSeconds dalay = new WaitForSeconds(_dalayUpdateTop);

        for (int i = 0; i < _nameLeaderbords.Length; i++)
        {
            YandexGame.NewLeaderboardScores(_nameLeaderbords[i], score[i]);

            yield return dalay;
        }
    }
}