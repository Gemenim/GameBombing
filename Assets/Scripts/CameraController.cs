using UnityEngine;
using YG;

[RequireComponent(typeof(Camera))]
public class CameraDistanceController : MonoBehaviour
{
    [SerializeField] private Transform _level;
    [SerializeField] private float _computerDistance;
    [SerializeField] private float _mobileDistance;

    [Header("Settings Camera Transform")]
    [SerializeField] private Vector3 _positionComputer;
    [SerializeField] private Vector3 _positionMobile;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();

        if (YandexGame.EnvironmentData.isDesktop)
        {
            _camera.fieldOfView = _computerDistance;
            _camera.transform.position = _positionComputer;
        }
        else if (YandexGame.EnvironmentData.isMobile)
        {
            _camera.fieldOfView = _mobileDistance;
            _camera.transform.position = _positionMobile;
        }
    }
}
