using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class CoreCube : Cube
{
    [SerializeField] private Color _targetColor;

    private const float c_hilthCore = 10.0f;
    private const int c_levelCoefficientCore = 2;

    private MeshRenderer _renderer;
    private Color _startColor;
    private Cube[] _allCubes;

    public int Level => _level;
    public event Action BlownUp;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _startColor = _renderer.material.color;
    }

    private void OnDestroy()
    {
        Debug.Log(_allCubes);
        foreach (Cube cube in _allCubes)
        {
            if (cube != null)
                cube.TakeDamage(cube.Hilth);
        }
    }

    public void SetCubes(Cube[] cubes)
    {
        _allCubes = cubes;
    }

    public override void CalculateStats()
    {
        _hilth = _defoltHilth * _level * _levelCoefficient + c_hilthCore * _level * c_levelCoefficientCore;
        _cost = _defoltCost * _level + _defoltCost * _level * c_levelCoefficientCore;
    }

    public void StartCountingDown(float countdownTime) => StartCoroutine(CountingDown(countdownTime));

    private IEnumerator CountingDown(float countdownTime)
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
