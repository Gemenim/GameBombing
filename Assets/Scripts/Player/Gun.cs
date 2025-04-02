using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Pool<Bullet> _pool;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Bullet _prefab;
    [SerializeField] private float _velocity;

    [SerializeField] private float _sensitivityHorizontal;
    [SerializeField] private float _sensitivityVertical;

    [SerializeField] private float _rateOfFire = 1.0f;
    [SerializeField] private float _startSpeedBullet = 1.0f;

    [SerializeField] private float _maxRotationZ;
    [SerializeField] private float _maxRotationW;

    private Camera _camera;
    private Transform _transform;

    private void OnValidate()
    {
        if (_rateOfFire <= 0)
            _rateOfFire = 1.0f;

        if (_startSpeedBullet <= 0)
            _startSpeedBullet = 1.0f;
    }

    private void Awake()
    {
        _camera = Camera.main;
        _transform = transform;
        _pool = new Pool<Bullet>(Preload, GetAction, ReturnAction);
    }

    public void Guidance(Vector2 mousePosition)
    {
        Vector3 mousePosition3D = mousePosition;
        mousePosition3D.z = 30f;
        Vector3 worldMousePosition = _camera.ScreenToWorldPoint(mousePosition3D);
        worldMousePosition.z = _transform.position.z;

        Vector3 difference = worldMousePosition - _transform.position;
        difference.Normalize();

        float angle = Mathf.Atan2(difference.x, difference.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, -angle);

        CheckGuidanceBoundaries();
    }

    public void Shoot()
    {
        Bullet bullet = _pool.Get();
        bullet.transform.position = _spawnPoint.position;
        bullet.SetDirection(_transform.up);
    }

    private void CheckGuidanceBoundaries()
    {
        if (_transform.rotation.z > _maxRotationZ && _transform.rotation.w < _maxRotationW)
            _transform.rotation = new Quaternion (0, 0, _maxRotationZ, _maxRotationW);
        else if (_transform.rotation.z < -_maxRotationZ && _transform.rotation.w < _maxRotationW)
            _transform.rotation = new Quaternion (0, 0, -_maxRotationZ, _maxRotationW);
    }
    private Bullet Preload()
    {
        Bullet bullet = Instantiate(_prefab);
        bullet.SetPool(_pool);
        bullet.SetVelocity(_velocity);

        return bullet;
    }

    private void ReturnAction(Bullet bullet)
    {
        bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        bullet.gameObject.SetActive(false);
    }

    private void GetAction(Bullet bullet) => bullet.gameObject.SetActive(true);
}
