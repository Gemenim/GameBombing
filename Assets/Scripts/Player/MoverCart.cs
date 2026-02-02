using UnityEngine;
using YG;

[RequireComponent(typeof(Rigidbody))]
public class MoverCart : MonoBehaviour
{
    [SerializeField] private PidRegulator _pidRegulator = new PidRegulator();
    [Range(0, 100)]
    [SerializeField] private Transform _rails;
    [SerializeField] private float _maxForce;
    [SerializeField] private Animator[] _animators;

    private Transform _transform;
    private Rigidbody _rigidbody;
    private float _minBarrier;
    private float _maxBarrier;
    private Vector3 _targetPosition;
    private float _speed;

    private void Awake()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();

        _minBarrier = _rails.position.x - _rails.localScale.x / 2;
        _maxBarrier = _rails.position.x + _rails.localScale.x / 2;


        Debug.Log(_minBarrier);
        Debug.Log(_maxBarrier);
    }

    private void Update()
    {
        if (YandexGame.isGamePlaying)
        {
            if (Time.deltaTime > 0)
            {
                Vector3 force = Vector3.right * _maxForce * _pidRegulator.Tick(_transform.position.x, _targetPosition.x, Time.deltaTime);

                _rigidbody.AddForce(force, ForceMode.Force);

                _speed = Vector3.Magnitude(_rigidbody.velocity);
                float direction = _rigidbody.velocity.normalized.x;

                foreach (Animator animator in _animators)
                {
                    animator.SetFloat("speed", _speed);
                   animator.SetFloat("velocityVector", _speed * direction);
                }
            }
        }
    }

    public float Move(float position)
    {
        if (position > _maxBarrier)
            return 0;
        else if (position < _minBarrier)
            return 0;

        _targetPosition = new Vector3(position, _transform.position.y, _transform.position.z);

        return _speed;
    }
}
