using System.Collections;
using UnityEngine;

public class BarrierMover : MonoBehaviour
{
    [SerializeField] private Transform[] _positions;
    [SerializeField] private float _angleRotation;
    [SerializeField] private float _timeAnimation;

    private Transform[] _barriers;

    private float[] _minPositionX;
    private float[] _maxPositionX;
    private float[] _minPositionY;
    private float[] _maxPositionY;

    private void OnValidate()
    {
        if (_timeAnimation <= 0)
            _timeAnimation = 20f;
    }

    private void Awake()
    {
        GetBariers();
        GetMaxMinPositions();
    }

    public void Move()
    {
        for (int i = 0; i < _positions.Length; i++)
        {            
            StartCoroutine(ToMove(_barriers[i], i));
            StartCoroutine(ToRotat(_positions[i]));
        }
    }

    private void GetMaxMinPositions()
    {
        _minPositionX = new float[_positions.Length];
        _maxPositionX = new float[_positions.Length];
        _minPositionY = new float[_positions.Length];
        _maxPositionY = new float[_positions.Length];

        for (int i = 0; i < _positions.Length; i++)
        {
            _minPositionX[i] = _positions[i].position.x - _positions[i].localScale.x / 2;
            _maxPositionX[i] = _positions[i].position.x + _positions[i].localScale.x / 2;
            _minPositionY[i] = _positions[i].position.y - _positions[i].localScale.y / 2;
            _maxPositionY[i] = _positions[i].position.y + _positions[i].localScale.y / 2;
        }
    }

    private void GetBariers()
    {
        _barriers = new Transform[_positions.Length];

        for (int i = 0; i < _positions.Length; i++)
            _barriers[i] = _positions[i].GetComponentInChildren<Transform>();
    }

    private IEnumerator ToMove(Transform transform, int id)
    {
        float randomPositionX = Random.Range(_minPositionX[id], _maxPositionX[id]);
        float randomPositionY = Random.Range(_minPositionY[id], _maxPositionY[id]);

        Vector3 newPosition = new Vector3(randomPositionX, randomPositionY, transform.position.z);

        float currentMovementTime = 0f;

        while (Vector3.Distance(transform.position, newPosition) > 0.002f)
        {
            currentMovementTime += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, newPosition, currentMovementTime / _timeAnimation);
            yield return null;
        }
    }

    private IEnumerator ToRotat(Transform transform)
    {
        float randomAngle = Random.Range(-_angleRotation, _angleRotation);

        Quaternion newAngle = Quaternion.Euler(0f, 0f, randomAngle);

        float currentMovementTime = 0f;

        while (transform.rotation != newAngle)
        {
            currentMovementTime += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(transform.localRotation, newAngle, currentMovementTime / _timeAnimation);
            yield return null;
        }
    }
}
