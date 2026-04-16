using System;
using UnityEngine;

public class Bomb : Chip
{
    [SerializeField] private GameObject[] _candidates;
    [SerializeField] private float _countdownTime;
    [SerializeField] private float _randomPositionZ = 0.25f;

    [Header("Settings 'TsarBomba'")]
    [SerializeField] private int _coefficientLevel;

    private CoreCube _core;
    private Cube[] _allCubes;

    public CoreCube Core => _core;

    public event Action Dastroy;

    private void OnValidate()
    {
        if (_countdownTime <= 0)
            _countdownTime = 60f;

        if (_coefficientLevel <= 0)
            _coefficientLevel = 1;
    }

    protected override void Awake()
    {
        int randomId = UnityEngine.Random.Range(0, _candidates.Length);
        _core = _candidates[randomId].AddComponent<CoreCube>();
        base.Awake();
        RememberAllCubes();
    }

    private void Start()
    {
        RandomizePositionZ();
        Collect();
    }

    private void OnEnable()
    {
        _core.BlownUp += Explode;
    }

    private void OnDisable()
    {
        _core.BlownUp -= Explode;
    }

    public void SetTimerView(TimerView timeView) => _core.SetTimerView(timeView);

    public void InitializeBomb(int level, bool isTsarBomb)
    {
        if (level > 1)
        {
            if (isTsarBomb)
            {
                _core.SetSetings(_allCubes, level, isTsarBomb);
                _core.StartCountdown(_countdownTime);
            }
            else
            {
                _core.SetSetings(_allCubes, level, isTsarBomb);
            }
        }
        else
        {
            _core.SetSetings(_allCubes, 1, isTsarBomb);

            if (isTsarBomb)
                _core.StartCountdown(_countdownTime);
        }
    }

    private void RememberAllCubes()
    {
        _allCubes = transform.GetComponentsInChildren<Cube>();
    }

    private void Explode() => Dastroy?.Invoke();

    [ContextMenu("Randomize Position Z")]
    private void RandomizePositionZ()
    {
        for (int i = 0; i < _allCubes.Length; i++)
        {
            Transform child = _allCubes[i].transform;
            Vector3 localPosition = child.localPosition;
            localPosition.z = UnityEngine.Random.Range(-_randomPositionZ, _randomPositionZ);
            child.localPosition = localPosition;
        }
    }
}