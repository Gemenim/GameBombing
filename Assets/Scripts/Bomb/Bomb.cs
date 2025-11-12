using System;
using UnityEngine;

public class Bomb : Chip
{
    [SerializeField] private CoreCube _core;
    [SerializeField] private float _countdownTime;
    [SerializeField] private float _randomPositionZ = 0.25f;

    [Header("Settings 'TsarBomba'")]
    [SerializeField] private int _coefficientLevel;

    private Cube[] _allCubes;

    public event Action Dastroy;

    private void OnValidate()
    {
        if (_countdownTime <= 0)
            _countdownTime = 60f;

        if (_coefficientLevel <= 0)
            _coefficientLevel = 1;
    }

    private void Start()
    {
        RememberAllCubes();
        Collect();
        RandomizePositionZ();
        _core.SetCubes(_allCubes);
        SetStats();
        _transform.parent.GetComponent<RanomazeBombs>().Randomaze();
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
        if (isTsarBomb)
        {
            _core.SetSetings(level * _coefficientLevel, isTsarBomb);
            _core.StartCountdown(_countdownTime);
        }
        else
        {
            _core.SetSetings(level, isTsarBomb);
        }
    }

    private void RememberAllCubes()
    {
        _allCubes = transform.GetComponentsInChildren<Cube>();
    }

    private void Explode() => Dastroy?.Invoke();

    private void SetStats()
    {
        foreach (Cube cube in _allCubes)
        {
            if (cube.name != _core.name)
            {
                cube.SetSetings(_core.Level, _core.IsTsar);
            }

            cube.CalculateStats();
        }
    }

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