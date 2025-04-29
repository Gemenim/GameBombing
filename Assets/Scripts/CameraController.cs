using UnityEngine;
using YG;

[RequireComponent(typeof(Camera))]
public class CameraDistanceDetector : MonoBehaviour
{
    [SerializeField] private float _computerDistance;
    [SerializeField] private float _mobileDistance;

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();

        if (YandexGame.EnvironmentData.isDesktop)
            _camera.fieldOfView = _computerDistance;
        else if (YandexGame.EnvironmentData.isMobile)
            _camera.fieldOfView = _mobileDistance;
    }
}
