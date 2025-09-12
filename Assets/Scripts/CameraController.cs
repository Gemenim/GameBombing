using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraDistanceController : MonoBehaviour
{
    [SerializeField]private Vector2 _defaultResolution = new Vector2(1920, 1080);

    [Range(0f, 1f)] 
    [SerializeField] private float _widthOrHeight = 0;
    [SerializeField] private float _ratio = 1f;

    private Camera _camera;
    private Transform _transform;

    private float _initialSize;
    private float _initialPositionY;
    private float _targetAspect;
    private float _positionX, _positionZ;

    private void OnValidate()
    {
        if (_ratio < 0)
            _ratio = 0;
    }

    private void Start()
    {
        _camera = GetComponent<Camera>();
        _transform = transform;
        _initialSize = _camera.orthographicSize;
        _initialPositionY = _transform.position.y;
        _positionX = _transform.position.x;
        _positionZ = _transform.position.z;

        _targetAspect = _defaultResolution.x / _defaultResolution.y;
    }

    private void Update()
    {
        if (_camera.orthographic)
        {
            float constantWidthSize = _initialSize * (_targetAspect / _camera.aspect);
            _camera.orthographicSize = Mathf.Lerp(constantWidthSize, _initialSize, _widthOrHeight);
            float ratioPosition = (_initialSize - _camera.orthographicSize) / _ratio;
            float positionY = -(ratioPosition - _initialPositionY);
            _transform.position = new Vector3(_positionX, positionY, _positionZ);
        }
    }

    private float CalcVerticalFov(float hFovInDeg, float aspectRatio)
    {
        float hFovInRads = hFovInDeg * Mathf.Deg2Rad;

        float vFovInRads = 2 * Mathf.Atan(Mathf.Tan(hFovInRads / 2) / aspectRatio);

        return vFovInRads * Mathf.Rad2Deg;
    }
}
