using UnityEngine;
using YG;

public class GameScaler : MonoBehaviour
{
    [SerializeField] private Transform _level;

    [Header("Level")]
    [SerializeField] private Vector3 _scaleComputer;
    [SerializeField] private Vector3 _scaleMobile;
    [SerializeField] private Vector3 _posiotionMobile;

    private void Awake()
    {
        if (YandexGame.EnvironmentData.isDesktop)
        {
            _level.localScale = _scaleComputer;
        }
        else if (YandexGame.EnvironmentData.isMobile)
        {
            _level.position = _posiotionMobile;
            _level.localScale = _scaleMobile;
        }
    }
}
