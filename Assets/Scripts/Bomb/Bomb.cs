using System;
using UnityEngine;

public class Bomb : Chip
{
    [SerializeField] private CoreCube _core;
    [SerializeField] private float _countdownTime;

    [Header("Settings 'TsarBomba'")]
    [SerializeField] private int _coefficientLevel;

    private Cube[] _allCubes;
    private bool _isTsar;

    public event Action<bool> Destroyed;

    private void Start()
    {
        SetStats();
        RememberAllCubes();
    }

    private void OnEnable()
    {
        _core.Destroyed += Destroy;
        _core.BlownUp += Explode;
    }

    private void OnDisable()
    {
        _core.Destroyed -= Destroy;
        _core.BlownUp -= Explode;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }

    public void InitializeBomb(int level, bool isTsarBomb)
    {
        _isTsar = isTsarBomb;

        if (_isTsar)
        {
            _core.SetLevel(level * _coefficientLevel);
            _core.StartCountingDown(_countdownTime);
        }
        else
        {
            _core.SetLevel(level);
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
        Destroyed?.Invoke(_isTsar);

        foreach (Cube cube in _allCubes)
        {
            if (cube != null)
                cube.TakeDamage(cube.Hilth);
        }

        RecalculateCubes();
    }

    private void Explode()
    {
        Destroy(this.gameObject);
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