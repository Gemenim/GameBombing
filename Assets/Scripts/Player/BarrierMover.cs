using UnityEngine;

public class BarrierMover : MonoBehaviour
{
    [SerializeField] Transform[] _positions;
    [SerializeField] float _angleRotation;

    private Transform[] _barriers;

    private float[] _minPositionX;
    private float[] _maxPositionX;
    private float[] _minPositionY;
    private float[] _maxPositionY;

    private void Awake()
    {
        GetBariers();
        GetMaxMinPositions();
    }

    public void Move()
    {
        for (int i = 0; i < _positions.Length; i++)
        {
            float randomPositionX = Random.Range(_minPositionX[i], _maxPositionX[i]);
            float randomPositionY = Random.Range(_minPositionY[i], _maxPositionY[i]);
            float randomAngle = Random.Range(-_angleRotation, _angleRotation);

            _barriers[i].position = new Vector3(randomPositionX, randomPositionY, _barriers[i].position.z);
            _positions[i].rotation = Quaternion.Euler(0f, 0f, randomAngle);
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
}
