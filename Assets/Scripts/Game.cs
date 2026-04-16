using System.Collections;
using UnityEngine;
using YG;

public class Game : MonoBehaviour
{
    private const float c_coefficientExperience = 0.45f;
    private const float c_ñoefficientNeedExperience = 0.01f;
    private const int c_multiplierLevel = 10;
    private const float c_defoltNeedExperience = 1500f;

    [SerializeField] Player _player;
    [SerializeField] private BombsGenerator _generator;
    [SerializeField] private CollectorCubes _collector;
    [SerializeField] private Sky _sky;
    [SerializeField] private BarrierMover _barrierMover;
    [SerializeField] private LevelBar _levelBar;
    [SerializeField] private TimerView _timerView;
    [SerializeField] private SavingHerald _savingHerald;

    [Header("Windows")]
    [SerializeField] private HudScreen _hudScreen;
    [SerializeField] private SettingsScreen _settingsScreen;
    [SerializeField] private UpgrateScreen _upgrateScreen;
    [SerializeField] private LeaderbordScreen _leaderbordScreen;

    [SerializeField] private MuteSourceButton _muteSourceButton;

    [SerializeField] private string[] _nameLeaderbords;

    [SerializeField] private Training _tutorial;

    private Bomb _bomb;
    private int _maxNumberAttempts = 5;
    private Coroutine _updateTop = null;
    private float _levelCoefficientNeedExperience = 2.435f;
    private float _dalayUpdateTop = 1f;

    public int Level { get; private set; } = 1;

    private void Start()
    {
        if (YandexGame.SDKEnabled)
            _savingHerald.Load();

        YandexGame.GameplayStart();
        _barrierMover.Move();

        if (_player.IsBeginner && Level == 1)
        {
            Spawn(false);
            _tutorial.StartScript(_bomb.Core);
        }
        else
        {
            Spawn(false);
        }
    }

    private void OnEnable()
    {
        _savingHerald.Saved += SaveData;
        _savingHerald.Uploaded += LoadSave;
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
        YandexGame.onGamePlaying += GameProcessSwitch;
    }

    private void OnDisable()
    {
        _savingHerald.Saved -= SaveData;
        _savingHerald.Uploaded -= LoadSave;
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
        YandexGame.onGamePlaying -= GameProcessSwitch;
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
        _sky.ClearBombs();
        _sky.ClearBullets();
        Spawn(true);
    }

    [ContextMenu("Bomb")]
    private void SpawnNowBomb()
    {
        _sky.ClearBombs();
        _sky.ClearBullets();
        Spawn(false);
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
        _sky.ClearBombs();
        _sky.ClearBullets();
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
        _savingHerald.Save();
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
            _bomb.transform.SetParent(_sky.Bombs);
            _bomb.SetParentCubes(_sky.Cubes);
            _bomb.Dastroy += RespawnBomb;
            _hudScreen.OnTimer();
        }
        else
        {
            _bomb = _generator.Spawn(Level, isTsarBomb);
            _bomb.transform.SetParent(_sky.Bombs);
            _bomb.SetParentCubes(_sky.Cubes);
            _hudScreen.OffTimer();
        }
    }

    private float GetNeedExperience()
    {
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
        YandexGame.savesData.Experience = _levelBar.Experience;

        StartUpdateTop();
    }

    private void ResetSeve()
    {
        _sky.Clear();

        YandexGame.ResetSaveProgress();
        _savingHerald.Load();
        _savingHerald.Save();
        Spawn(false);
    }

    private void LoadSave()
    {
        Level = YandexGame.savesData.LevelGame;
        GetCoefficientNeedExperience();
        _levelBar.SetNeedExperience(GetNeedExperience(), Level);
        _levelBar.AddExperience(YandexGame.savesData.Experience);
        StartUpdateTop();
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

    private void GameProcessSwitch(bool isGamePlaying)
    {
        if (isGamePlaying)
        {
            Time.timeScale = 1.0f;
        }
        else
        {
            Time.timeScale = 0;
        }
    }
}