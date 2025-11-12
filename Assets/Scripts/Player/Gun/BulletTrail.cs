using System;
using System.Collections;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    [SerializeField] private ParticleSystem _trail;

    private Transform _transform;
    private Transform _parent;

    public event Action Return;

    private void Awake()
    {
        _transform = transform;
        _parent = _transform.parent;
    }

    public void UnfastenIt()
    {
        _transform.parent = null;

        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        while (_trail.particleCount > 0)
            yield return null;

        _transform.parent = _parent;
        Return?.Invoke();
    }
}
