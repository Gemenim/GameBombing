using UnityEngine;
using UnityEngine.UI;
using YG;

public class Game : MonoBehaviour
{
    [SerializeField] Player _player;
    [SerializeField] private BombsGenerator _generator;
    [SerializeField] private HudScreen _hudScreen;
    [SerializeField] private CollectorCubes _collector;
    [SerializeField] private ViewBar _viewBar;

    [Header("Windows")]
    [SerializeField] private SettingsScreen _settingsScreen;
    [SerializeField] private UpgrateScreen _upgrateScreen;

    private const float _levelCoefficientExperience = 0.5f;
    private const float _levelCoefficientNeedExperience = 1.5f;
    private const float _standartNeedExperience = 20f;

    private Bomb _bomb;
    private int _level = 1;

    public int Level => _level;

    private float _startNeedExperience => _standartNeedExperience * _level * _levelCoefficientNeedExperience;

    private void Start()
    {
        _viewBar.SetNeedExperience(_startNeedExperience, _level);
        _bomb = _generator.Spawn(_level, false);
    }

    private void OnEnable()
    {
        _hudScreen.OnUpgradeButtonClicked += OpenUpgradeScreen;
        _hudScreen.OnSetingsButtonClicked += OpenSettings;
        _settingsScreen.OnExitButtonClicked += Quit;
        _upgrateScreen.OnReturnButtonClicked += CloseUpgradeScreen;
        _settingsScreen.OnReturnButtonClicked += CloseSettings;
        _viewBar.OnButtonClicked += LevelUp;
        _collector.PutCoins += GetExperience;
    }

    private void OnDisable()
    {
        _hudScreen.OnUpgradeButtonClicked -= OpenUpgradeScreen;
        _hudScreen.OnSetingsButtonClicked -= OpenSettings;
        _settingsScreen.OnExitButtonClicked -= Quit;
        _settingsScreen.OnReturnButtonClicked -= CloseSettings;
        _upgrateScreen.OnReturnButtonClicked -= CloseUpgradeScreen;
        _viewBar.OnButtonClicked -= LevelUp;
        _collector.PutCoins -= GetExperience;
    }

    private void Update()
    {
        if (_bomb == null)
            Spawn();
    }

    private void OpenUpgradeScreen()
    {
        Debug.Log("GoOpen");
        _upgrateScreen.Open();
        Time.timeScale = 0;
    }

    private void CloseUpgradeScreen()
    {
        Debug.Log("Go");
        _upgrateScreen.Close();
        Time.timeScale = 1.0f;
    }

    private void OpenSettings()
    {
        _settingsScreen.Open();
        Time.timeScale = 0;
    }

    private void CloseSettings()
    {
        _settingsScreen.Close();
        Time.timeScale = 1f;
    }

    private void Quit()
    {
        Application.Quit();
    }

    private void LevelUp()
    {
        _level += 1;
        _viewBar.SetNeedExperience(_startNeedExperience, _level);
    }

    private void GetExperience(double coins)
    {
        double experience = (coins * _level * _levelCoefficientExperience) / 100;
        _viewBar.SetValue(experience);
    }

    private void Spawn()
    {
        _bomb = _generator.Spawn(_level, false);
    }
}
