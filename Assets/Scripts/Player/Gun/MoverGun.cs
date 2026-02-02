using UnityEngine;

public class MoverGun : MonoBehaviour
{
    private const float c_maxRotationZ = 0.66f;
    private const float c_maxRotationW = 0.74f;

    [SerializeField] private float _multiplaySpeed;
    [SerializeField] private float _standartSpeed;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    public void Move(float speed)
    {
        _transform.rotation *= Quaternion.Euler(0, 0, Mathf.Abs(speed) * _multiplaySpeed + _standartSpeed);

        if (_transform.rotation.z > c_maxRotationZ && _transform.rotation.w < c_maxRotationW)
            ChangeDirection();
        else if (_transform.rotation.z < -c_maxRotationZ && _transform.rotation.w < c_maxRotationW)
            ChangeDirection();
    }

    private void ChangeDirection()
    {
        _multiplaySpeed *= -1;
        _standartSpeed *= -1;
    }
}
