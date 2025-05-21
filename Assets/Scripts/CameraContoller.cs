using UnityEngine;
using YG;

public class CameraContoller : MonoBehaviour
{
    [SerializeField] private Vector3 _psitionMobile;
    [SerializeField] private float _sizeMobile;

    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;

        if (YandexGame.EnvironmentData.isMobile)
        {
            _camera.orthographicSize = _sizeMobile;
            _camera.transform.position = _psitionMobile;
        }
    }
}
