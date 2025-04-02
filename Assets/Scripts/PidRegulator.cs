using UnityEngine;

[System.Serializable]
public class PidRegulator
{
    [SerializeField] private float _ki = 1;
    [SerializeField] private float _kp = 1;
    [SerializeField] private float _kd = 1;
    [SerializeField] private float _minOut = -1000;
    [SerializeField] private float _maxOut = 1000;

    private float _integral, _prevError, _error, _currentDelta, _targetPint;

    public float Tick(float input, float time)
    {
        return ComputePID(time, input);
    }

    public float Tick(float input, float target, float time)
    {
        SetTarget(target);
        return ComputePID(time, input);
    }

    public void SetTarget(float target)
    {
        _targetPint = target;
    }

    public void SetMinMax(float min, float max)
    {
        _minOut = min;
        _maxOut = max;
    }

    private float ComputePID(float time, float input)
    {
        _error = _targetPint - input;
        _integral = Mathf.Clamp(_integral + _error * time * _ki, _minOut, _maxOut);
        _currentDelta = (_error - _prevError) / time;
        _prevError = _error;
        return Mathf.Clamp(_error * _kp + _integral + _currentDelta * _kd, _minOut, _maxOut);
    }
}