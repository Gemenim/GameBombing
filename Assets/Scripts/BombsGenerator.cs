using UnityEngine;

public class BombsGenerator : MonoBehaviour
{
    [SerializeField] private Bomb[] _bombsPrefabs;
    [SerializeField] private float _spreadPositionX;
    [SerializeField] private float _minAngle, _maxAngle;

    private Transform _spawnPoint;
    private float _maxPositionX, _minPositionX;

    private void Awake()
    {
        _spawnPoint = transform;
        _maxPositionX = _spawnPoint.position.x + _spreadPositionX;
        _minPositionX = _spawnPoint.position.x - _spreadPositionX;
    }

    public Bomb Spawn(int level, bool isTsarBomb)
    {
        int index = Random.Range(0, _bombsPrefabs.Length);
        Bomb bomb = _bombsPrefabs[index];
        Bomb newBomb = Instantiate(bomb, GetRandomPosition(), GetRandomQuternion());

        newBomb.InitializeBomb(level, isTsarBomb);
        return newBomb;
    }

    public Bomb Spawn(int level, bool isTsarBomb, TimerView timer)
    {
        int index = Random.Range(0, _bombsPrefabs.Length);
        Bomb bomb = _bombsPrefabs[index];
        Bomb newBomb = Instantiate(bomb, GetRandomPosition(), GetRandomQuternion());

        newBomb.SetTimerView(timer);
        newBomb.InitializeBomb(level, isTsarBomb);

        return newBomb;
    }

    private Quaternion GetRandomQuternion()
    {
        float randomAngle = 0;

        while (-_minAngle < randomAngle && randomAngle < _minAngle)
            randomAngle = Random.Range(-_maxAngle, _maxAngle);

        Quaternion randomQuaternion = Quaternion.Euler(0, 0, randomAngle);

        return randomQuaternion;
    }

    private Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(_minPositionX, _maxPositionX);
        Vector3 position = new Vector3(randomX, _spawnPoint.position.y, _spawnPoint.position.z);

        return position;
    }
}
