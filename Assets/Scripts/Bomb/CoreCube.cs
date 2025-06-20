using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class CoreCube : Cube
{
    [SerializeField] private Color _targetColor;

    private const float c_hilthCore = 10.0f;
    private const float c_levelCoefficientCore = 1.5f;

    private MeshRenderer _renderer;
    private Color _startColor;
    private Cube[] _allCubes;
    private Coroutine _coroutineCountdown;

    public int Level => _level;
    public event Action BlownUp;

    protected override void Awake()
    {
        base.Awake();
        _renderer = GetComponent<MeshRenderer>();
        _startColor = _renderer.material.color;
    }

    public void SetCubes(Cube[] cubes)
    {
        _allCubes = cubes;
    }

    public override void CalculateStats()
    {       
        base.CalculateStats();

        Hilth += (c_hilthCore * Mathf.Pow(_level, c_levelCoefficientCore) + (c_hilthCore * _level));
        Cost += (_defoltCost * Mathf.Pow(_level, c_levelCoefficientCore) + (_defoltCost * _level));
    }

    public override void StartDastroy()
    {
        base.StartDastroy();
        BreakItUpBomb();

        if (_coroutineCountdown != null)
            StopCoroutine(_coroutineCountdown);
    }

    public void StartCountdown(float countdownTime) => _coroutineCountdown = StartCoroutine(Countdown(countdownTime));

    private void BreakItUpBomb()
    {
        foreach (Cube cube in _allCubes)
        {
            if (cube != null)
            {
                cube.TakeDamage(cube.Hilth);
            }
        }
    }

    private IEnumerator Countdown(float countdownTime)
    {
        float time = 0;

        while (time < countdownTime)
        {
            time += Time.deltaTime;
            Debug.Log(time);
            _renderer.material.color = Color.Lerp(_startColor, _targetColor, Mathf.PingPong(Time.deltaTime, 1));

            if (time >= countdownTime)
                BlownUp?.Invoke();

            yield return null;
        }
    }
}
