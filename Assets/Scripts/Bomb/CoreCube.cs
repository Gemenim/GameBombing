using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Cube))]
[RequireComponent(typeof(LightAlarm))]
[RequireComponent(typeof(ColorCube))]
public class CoreCube : MonoBehaviour
{
    private const int c_levelCoefficientCore = 1;

    private Cube _cube;
    private LightAlarm _lightAlarm;
    private Cube[] _allCubes;
    private TimerView _timer;
    private Coroutine _coroutineCountdown;

    public int Level { get; private set; }
    public bool IsTsar { get; private set; }

    public event Action BlownUp;

    private void Awake()
    {
        _cube = GetComponent<Cube>();
        _lightAlarm = GetComponent<LightAlarm>();
    }

    private void Update()
    {
        if (_coroutineCountdown != null)
        {
            if (_cube.IsDetouch)
            {
                _lightAlarm.Stop();
                StopCoroutine(_coroutineCountdown);
            }
        }

        if (_cube.IsColect)
            BreakItUpBomb();
    }

    public void SetSetings(Cube[] cubes, int level, bool isTsar)
    {
        _allCubes = cubes;
        Level = level;
        IsTsar = isTsar;

        foreach (Cube cube in _allCubes)
        {
            if (cube == _cube)
                cube.SetSetings(Level + c_levelCoefficientCore, isTsar);
            else
                cube.SetSetings(Level, IsTsar);
        }
    }

    public void SetTimerView(TimerView timerView) => _timer = timerView;

    public void StartCountdown(float countdownTime) => _coroutineCountdown = StartCoroutine(Countdown(countdownTime));

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