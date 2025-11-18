using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ColorCube))]
public class CoreCube : Cube
{
    [SerializeField] private LightAlarm _lightAlarm;

    private const float c_hilthCore = 2.5f;
    private const float c_levelCoefficientCore = 1.5f;

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

        _hilth += LevelCalculator.Calculat(c_hilthCore, c_levelCoefficientCore, _level);
        Cost += LevelCalculator.Calculat(c_defoltCost, c_levelCoefficientCore, _level);
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

    public override void Detouch()
    {
        _lightAlarm.Stop();
        base.Detouch();

        if (_coroutineCountdown != null)
            StopCoroutine(_coroutineCountdown);
    }

    private void BreakItUpBomb()
    {
        foreach (Cube cube in _allCubes)
            if (cube != null)
                cube.Detouch();
    }

    private IEnumerator Countdown(float countdownTime)
    {
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