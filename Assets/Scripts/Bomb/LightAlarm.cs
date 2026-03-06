using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ColorCube))]
public class LightAlarm : MonoBehaviour
{
    [SerializeField] private Color _startColor, _endColor;

    private ColorCube _colorCube;
    private float _dalay = 1f;
    private Coroutine _blink;

    private void Awake()
    {
        _colorCube = GetComponent<ColorCube>();
    }

    public void Start()
    {
        _blink = StartCoroutine(Blink());
    }

    public void Stop()
    {
        StopCoroutine(_blink);
        _colorCube.ApplyColor(_startColor);
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            _colorCube.ApplyColor(Color.Lerp(_startColor, _endColor, Mathf.Abs(Mathf.Sin(Time.time * _dalay))));
            yield return null;
        }
    }
}
