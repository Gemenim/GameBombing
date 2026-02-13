using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Cube))]
[RequireComponent(typeof(LightAlarm))]
[RequireComponent(typeof(ColorCube))]
public class CoreCube : MonoBehaviour
{
    private const float c_hilthCore = 2.5f;
    private const float c_levelCoefficientCore = 1.5f;

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

    public void SetCubes(Cube[] cubes)
    {
        _allCubes = cubes;
    }

    public void SetSetings(int level, bool isTsar)
    {
        Level = level;
        IsTsar = isTsar;
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