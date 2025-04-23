using UnityEngine;
using YG;

[RequireComponent(typeof(Rigidbody))]
public class MoverCart : MonoBehaviour
{
    [SerializeField] PidRegulator _pidRegulator = new PidRegulator();
    [SerializeField] private Transform _rails;
    [SerializeField] private float _maxForce;

    private Transform _transform;
    private Rigidbody _rigidbody;
    private float _minBarrier;
    private float _maxBarrier;
    private float _targetPositionX;

    private void Awake()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();

        _minBarrier = _rails.position.x - _rails.localScale.x / 2;
        _maxBarrier = _rails.position.x + _rails.localScale.x / 2;
    }

    private void Update()
    {
        if (YandexGame.isGamePlaying)
        {
            Vector3 force = Vector3.right * _maxForce * _pidRegulator.Tick(_transform.position.x, _targetPositionX, Time.deltaTime);
            _rigidbody.AddForce(force, ForceMode.Force);
        }
    }

    public void MoveCar()
    {
        _targetPositionX = Random.Range(_minBarrier, _maxBarrier);
    }
}
