using System.Collections.Generic;
using UnityEngine;
using YG;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody))]
public class MoverPlatform : MonoBehaviour
{
    [SerializeField] private PidRegulator _pidRegulator = new PidRegulator();
    [SerializeField] private Transform _rails;
    [SerializeField] private float _maxForce;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private Animator[] _animators;

    private Transform _transform;
    private Rigidbody _rigidbody;
    private Transform _target = null;
    private Vector3 _force;
    private float _startPosition;
    private float _endPosition;
    private float _speed;

    private void Awake()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody>();

        _startPosition = _rails.position.x - _rails.localScale.x / 2 + _transform.localScale.x;
        _endPosition = _rails.position.x + _rails.localScale.x / 2 - _transform.localScale.x;

        RetornEndPosint();
    }

    private void Update()
    {
        if (YandexGame.isGamePlaying)
        {
            if (Time.deltaTime > 0)
            {
                if (_target == null)
                    _force = Vector3.right * _maxForce * _pidRegulator.Tick(_transform.position.x, _endPosition, Time.deltaTime);
                else
                    _force = Vector3.right * _maxForce * _pidRegulator.Tick(_transform.position.x, _target.position.x, Time.deltaTime);

                if (_rigidbody.velocity.x < _maxSpeed)
                    _rigidbody.AddForce(_force, ForceMode.Force);

                if (_rigidbody.velocity.x >= _maxSpeed && _target == null)
                    _rigidbody.velocity = new Vector3(_maxSpeed, _rigidbody.velocity.y, _rigidbody.velocity.z);

                _speed = Vector3.Magnitude(_rigidbody.velocity);
                float direction = _rigidbody.velocity.normalized.x;

                foreach (Animator animator in _animators)
                {
                    animator.SetFloat("speed", _speed);
                    animator.SetFloat("velocityVector", _speed * direction);
                }

                if (Vector3.Distance(_transform.position, new Vector3(_endPosition, _transform.position.y, _transform.position.z)) < 0.5f)
                {
                    float temporaryVector = _endPosition;
                    _endPosition = _startPosition;
                    _startPosition = temporaryVector;
                    RetornEndPosint();
                }
            }
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        Debug.Log("GetRarget");
    }

    public void RetornEndPosint()
    {
        _target = null;
        Debug.Log("return");
    }
}