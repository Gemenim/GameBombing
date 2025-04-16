using System;

public class CoreCube : Cube
{
    private const float c_hilthCore = 10.0f;
    private const int c_levelCoefficientCore = 2;

    public int Level => _level;
    public event Action Destroyed;

    private void OnDestroy()
    {
        Destroyed?.Invoke();
    }

    public override void SetLevel(int level)
    {
        int randomLevel = level + UnityEngine.Random.Range(-2, 2);
        _level = randomLevel > 0 ? randomLevel : 1;
    }

    public override void CalculateStats()
    {
        _hilth = _defoltHilth * _level * _levelCoefficient + c_hilthCore * _level * c_levelCoefficientCore;
        _cost = _defoltCost * _level + _defoltCost * _level * c_levelCoefficientCore;
    }
}
