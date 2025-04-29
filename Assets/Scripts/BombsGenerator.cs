using UnityEngine;

public class BombsGenerator : MonoBehaviour
{
    [SerializeField] private Bomb[] _bombsPrefabs;

    private Transform _spawnPoint;
    private float _maxSpawnPositionX;
    private float _minSpawnPositionX;

    private void Awake()
    {
        _spawnPoint = transform;
        _maxSpawnPositionX = _spawnPoint.position.x + _spawnPoint.localScale.x / 2;
        _minSpawnPositionX = _spawnPoint.position.x - _spawnPoint.localScale.x / 2;
    }

    public Bomb Spawn(int level, bool isTsarBomb)
    {
        int index = Random.Range(0, _bombsPrefabs.Length);
        Bomb bomb = _bombsPrefabs[index];
        Bomb newBomb = Instantiate(bomb, GetRandomPosition(), Quaternion.identity);

        newBomb.InitializeBomb(level, isTsarBomb);
        return newBomb;
    }

    private Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(_minSpawnPositionX, _maxSpawnPositionX);
        Vector3 position = new Vector3(randomX, _spawnPoint.position.y, _spawnPoint.position.z);

        return position;
    }
}
