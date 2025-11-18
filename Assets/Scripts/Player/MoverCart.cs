using UnityEngine;
using YG;

[RequireComponent(typeof(Rigidbody))]
public class MoverCart : MonoBehaviour
{
    [SerializeField] private PidRegulator _pidRegulator = new PidRegulator();
    [Range(0, 100)]
    [SerializeField] private float _maxSpeed;
    [SerializeField] private Transform _rails;
    [SerializeField] private float _maxForce;
    [SerializeField] private float _minRange;
    [SerializeField] private Animator[] _animators;

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
            if (Time.deltaTime > 0)
            {
                Vector3 force = Vector3.right * _maxForce * _pidRegulator.Tick(_transform.position.x, _targetPositionX, Time.deltaTime);

                if (_rigidbody.velocity.x < 50f)
                    _rigidbody.AddForce(force, ForceMode.Force);

                float speed = Vector3.Magnitude(_rigidbody.velocity);
                float direction = _rigidbody.velocity.normalized.x;

                foreach (Animator animator in _animators)
                {
                    animator.SetFloat("speed", speed);
                    animator.SetFloat("velocityVector", speed * direction);
                }
            }
        }
    }

    public void MoveCar()
    {
        float randomPosition = GetRandomPosition();

        while (Mathf.Abs(_targetPositionX - randomPosition) < _minRange)
            randomPosition = GetRandomPosition();

        _targetPositionX = randomPosition;
    }

    private float GetRandomPosition()
    {
        return Random.Range(_minBarrier, _maxBarrier);
    }
}
