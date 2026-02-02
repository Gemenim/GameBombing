using System.Collections;
using UnityEngine;

public class BombsGenerator : MonoBehaviour
{
    [SerializeField] private Transform _skyBomb;
    [SerializeField] private Bomb[] _bombsPrefabs;
    [SerializeField] private float _spreadPositionX;
    [SerializeField] private float _minAngle, _maxAngle;

    private Transform _transform;
    private Transform[] _spawnPoint;
    private Chip[] _chips;
    private System.Random _random = new System.Random();

    private void Awake()
    {
        _transform = transform;
        _spawnPoint = GetComponentsInChildren<Transform>();
        Mix();
    }

    public Bomb Spawn(int level, bool isTsarBomb)
    {
        Mix();
        int index = Random.Range(0, _bombsPrefabs.Length);
        Bomb bomb = _bombsPrefabs[index];
        Bomb newBomb = Instantiate(bomb, _transform.position, Quaternion.identity);

        newBomb.InitializeBomb(level, isTsarBomb);

        StartCoroutine(Exchange());

        return newBomb;
    }

    public Bomb Spawn(int level, bool isTsarBomb, TimerView timer)
    {
        Mix();
        int index = Random.Range(0, _bombsPrefabs.Length);
        Bomb bomb = _bombsPrefabs[index];
        Bomb newBomb = Instantiate(bomb, _transform.position, GetRandomQuternion());

        newBomb.SetTimerView(timer);
        newBomb.InitializeBomb(level, isTsarBomb);

        StartCoroutine(Exchange());

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

    public void Randomaze()
    {
        StartCoroutine(Exchange());
    }

    private void Mix()
    {
        for (int i = _spawnPoint.Length - 1; i >= 1; i--)
        {
            int j = _random.Next(i + 1);
            Transform temp = _spawnPoint[j];
            _spawnPoint[j] = _spawnPoint[i];
            _spawnPoint[i] = temp;
        }
    }

    private IEnumerator Exchange()
    {
        yield return null;

        _chips = _skyBomb.GetComponentsInChildren<Chip>();

        for (int i = 0; i < _chips.Length; i++)
        {
            _chips[i].transform.position = _spawnPoint[i].transform.position;
            _chips[i].transform.rotation = GetRandomQuternion();
        }
    }
}
