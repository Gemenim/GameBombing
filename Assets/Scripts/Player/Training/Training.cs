using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using YG;

public class Training : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _buttonWallet;
    [SerializeField] private GameObject _buttonUpgradeDeamge;
    [SerializeField] private GameObject _buttonUpgradeExplosion;
    [SerializeField] private LevelBar _levelBar;
    [SerializeField] private CanvasGroup _canvasGroup;
    [Header("Triger")]
    [SerializeField] private Player _player;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private UpgrateScreen _upgrateScreen;
    [SerializeField] private GameObject _closeUpgarteButton;
    [Header("Image Indicator")]
    [SerializeField] private GameObject _indicator;
    [SerializeField] private GameObject _control;
    [SerializeField] private GameObject _indicatorCore;

    private PlayerInput _playerInput;
    private CoreCube _coreCube;
    private ViewButtonUpgrade _viewButtonUpgradeDamage;
    private ViewButtonUpgrade _viewButtonUpgradeExplosion;
    private Vector3 _positionIndicator = new Vector3(-3, 3, 0);
    private Transform _paretButton;
    private int _siblingIndex;
    private Button _buttonUpgrade;

    private bool _isLaunched = false;
    private bool _isTutorilaUpgrade = false;

    private void Awake()
    {
        _playerInput = _player.GetInput();
        _viewButtonUpgradeDamage = _buttonUpgradeDeamge.GetComponent<ViewButtonUpgrade>();
        _viewButtonUpgradeExplosion = _buttonUpgradeExplosion.GetComponent<ViewButtonUpgrade>();
    }

    private void OnDisable()
    {
        _playerInput.Player.TutorialClick.performed -= StopControlTutorial;
    }

    public void StartScript(CoreCube coreCube)
    {
        YandexGame.GameplayStop();
        _coreCube = coreCube;
        _isLaunched = true;
        _isTutorilaUpgrade = false;
        _wallet.ChangeCount += CheckTriggerCoins;

        OnCanvas();
        _control.SetActive(true);
        _playerInput.Player.TutorialClick.performed += StopControlTutorial;
        _levelBar.GainedExperience += ShowCore;
        _levelBar.FilledUp += ShowLevelUp;
    }

    public void StoptScript()
    {
        _isLaunched = false;
    }

    //Core
    private void ShowCore()
    {
        YandexGame.GameplayStop();
        OnCanvas();
        _indicatorCore.SetActive(true);
        _playerInput.Player.TutorialClick.performed += StopShowCore;
        _indicatorCore.transform.position = _coreCube.transform.position;
        _indicatorCore.transform.localPosition = new Vector3(_indicatorCore.transform.localPosition.x, _indicatorCore.transform.localPosition.y, 0);
    }

    private void StopShowCore(InputAction.CallbackContext context)
    {
        _levelBar.GainedExperience -= ShowCore;
        YandexGame.GameplayStart();
        _indicatorCore.SetActive(false);
        OnCanvas(false);
        _playerInput.Player.TutorialClick.performed -= StopShowCore;
    }

    //Upgrade
    private void CheckTriggerCoins(float coins)
    {
        if (!_isLaunched) return;

        if (coins > _viewButtonUpgradeDamage.Cost && !_isTutorilaUpgrade)
        {
            OnTrainingUpgrade(_buttonUpgradeDeamge.GetComponent<Button>());
            _buttonWallet.GetComponent<Button>().onClick.AddListener(OnClickWallet);
            _isTutorilaUpgrade = true;
        }
        else if (coins > _viewButtonUpgradeExplosion.Cost && _isTutorilaUpgrade)
        {
            OnTrainingUpgrade(_buttonUpgradeExplosion.GetComponent<Button>());
            _buttonWallet.GetComponent<Button>().onClick.AddListener(OnClickWallet);
            _wallet.ChangeCount -= CheckTriggerCoins;
        }
    }

    private void OnClickWallet()
    {
        _buttonWallet.GetComponent<Button>().onClick.RemoveListener(OnClickWallet);
        ChangeButton(_buttonUpgrade.transform, _buttonWallet.transform);
        _buttonUpgrade.onClick.AddListener(OnClickUpgrade);
    }

    private void OnClickUpgrade()
    {
        _buttonUpgrade.onClick.RemoveListener(OnClickUpgrade);
        ChangeButton(_closeUpgarteButton.transform, _buttonUpgrade.transform);
        _closeUpgarteButton.GetComponent<Button>().onClick.AddListener(CloseUpgarde);
    }

    private void CloseUpgarde()
    {
        _closeUpgarteButton.transform.SetParent(_paretButton);
        _closeUpgarteButton.GetComponent<Button>().onClick.RemoveListener(CloseUpgarde);
        OnCanvas(false);
        _indicator.SetActive(false);
        YandexGame.GameplayStart();
    }

    //Control
    private void StopControlTutorial(InputAction.CallbackContext context)
    {
        YandexGame.GameplayStart();
        _control.SetActive(false);
        OnCanvas(false);
        _playerInput.Player.TutorialClick.performed -= StopControlTutorial;
    }

    //Boss
    private void ShowLevelUp()
    {
        YandexGame.GameplayStop();
        _levelBar.FilledUp -= ShowLevelUp;
        OnCanvas();
        _indicator.SetActive(true);
        ChangeButton(_levelBar.transform);
        _levelBar.OnButtonClicked += StopShowLevelUp;
    }

    private void StopShowLevelUp()
    {
        YandexGame.GameplayStart();
        _levelBar.transform.SetParent(_paretButton);
        _indicator.SetActive(false);
        OnCanvas(false);
        _levelBar.OnButtonClicked -= StopShowLevelUp;
    }

    //Systm
    private void ChangeButton(Transform newButton, Transform nowButton = null)
    {
        if (nowButton != null)
        {
            nowButton.transform.SetParent(_paretButton);
            nowButton.transform.SetSiblingIndex(_siblingIndex);
        }

        _paretButton = newButton.parent;
        _siblingIndex = newButton.GetSiblingIndex();
        _indicator.transform.position = newButton.position - _positionIndicator;
        newButton.SetParent(_canvasGroup.transform);
        newButton.SetSiblingIndex(0);
    }

    private void OnCanvas(bool isActive = true)
    {
        if (isActive)
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
        }
        else
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
        }
    }

    private void OnTrainingUpgrade(Button button)
    {
        YandexGame.GameplayStop();
        OnCanvas();
        _indicator.SetActive(true);
        ChangeButton(_buttonWallet.transform);
        _buttonUpgrade = button;
    }
}
