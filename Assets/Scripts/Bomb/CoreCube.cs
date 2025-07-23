using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ColorCube))]
public class CoreCube : Cube
{
    [SerializeField] private Color _targetColor;

    private const float c_hilthCore = 2.0f;
    private const float c_levelCoefficientCore = 1.7f;

    private Cube[] _allCubes;
    private TimerView _timer;
    private Coroutine _coroutineCountdown;

    public int Level => _level;
    public event Action BlownUp;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetCubes(Cube[] cubes)
    {
        _allCubes = cubes;
    }

    public override void CalculateStats()
    {       
        base.CalculateStats();

        Hilth += (c_hilthCore * Mathf.Pow(_level, c_levelCoefficientCore) - (c_hilthCore * _level));
        Cost += (c_defoltCost * Mathf.Pow(_level, c_levelCoefficientCore) - (c_defoltCost * _level));
    }

    public void SetTimerView(TimerView timerView) => _timer = timerView;

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
        Debug.Log(_timer);

        while (countdownTime > 0)
        {
            countdownTime -= Time.deltaTime;
            _timer.TextChange(countdownTime);

            if (countdownTime <= 0)
                BlownUp?.Invoke();

            yield return null;
        }
    }
}
