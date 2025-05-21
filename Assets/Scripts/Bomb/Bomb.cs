using System;
using UnityEngine;

public class Bomb : Chip
{
    [SerializeField] private CoreCube _core;
    [SerializeField] private float _countdownTime;

    [Header("Settings 'TsarBomba'")]
    [SerializeField] private int _coefficientLevel;

    private Cube[] _allCubes;

    public event Action<bool> Destroyed;

    private void Start()
    {
        SetStats();
        RememberAllCubes();
    }

    private void OnEnable()
    {
        //_core.Destroyed += Destroy;
        _core.BlownUp += Explode;
    }

    private void OnDisable()
    {
        //_core.Destroyed -= Destroy;
        _core.BlownUp -= Explode;
    }

    public void InitializeBomb(int level, bool isTsarBomb)
    {
        if (isTsarBomb)
        {
            _core.SetSetings(level * _coefficientLevel, isTsarBomb);
            _core.SetCubes(_cubes);
            _core.StartCountingDown(_countdownTime);
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

    private void Destroy()
    {
        foreach (Cube cube in _allCubes)
        {
            if (cube != null)
                cube.TakeDamage(cube.Hilth);
        }

        RecalculateCubes();
    }

    private void Explode()
    {
        Destroyed?.Invoke(false);
        Destroy(this.gameObject);
    }

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