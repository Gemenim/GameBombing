using System;
using UnityEngine;

public class Bomb : Chip
{
    [SerializeField] private CoreCube _core;

    private Cube[] _allCubes;

    public event Action Destroyed;

    private void Start()
    {
        SetStats();
        SetAllCubes();
    }

    private void OnEnable()
    {
        _core.Destroyed += Destroy;
    }

    private void OnDisable()
    {
        _core.Destroyed -= Destroy;
    }

    public void SetLevel(int level) => _core.SetLevel(level);

    private void SetAllCubes()
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

        Destroyed?.Invoke();
    }

    private void SetStats()
    {
        foreach (Cube cube in _cubes)
        {
            if (cube.name != _core.name)
            {
                cube.SetLevel(_core.Level);
            }

            cube.CalculateStats();
        }
    }
}