using System.Collections;
using UnityEngine;

public class LightAlarm : MonoBehaviour
{
    [SerializeField] private ColorCube _colorCube;
    [SerializeField] private Color _startColor, _endColor;
    [SerializeField] private float _dalay;

    private Coroutine _blink;

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
