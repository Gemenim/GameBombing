using System;
using UnityEngine;

public class Bomb : Chip
{
    [SerializeField] private CoreCube _core;
    [SerializeField] private float _countdownTime;

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
        SetStats();
        RememberAllCubes();
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
            _core.SetCubes(_cubes);
            _core.StartCountdown(_countdownTime);
        }
        else
        {
            _core.SetSetings(level, isTsarBomb);
            _core.SetCubes(_cubes);
        }
    }

    private void RememberAllCubes()
    {
        _allCubes = new Cube[_cubes.Length];

        for (int i = 0; i < _cubes.Length; i++)
        {
            _allCubes[i] = _cubes[i];
        }
    }

    private void Explode() => Dastroy?.Invoke();

    private void SetStats()
    {
        foreach (Cube cube in _cubes)
        {
            if (cube.name != _core.name)
            {
                cube.SetSetings(_core.Level, _core.IsTsar);
            }

            cube.CalculateStats();
        }
    }
}