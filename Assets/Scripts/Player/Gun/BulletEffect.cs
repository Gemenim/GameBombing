using System;
using System.Collections;
using UnityEngine;

public class BulletEffect : MonoBehaviour
{
    [SerializeField] protected ParticleSystem _particleSystem;

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
        while (_particleSystem.particleCount > 0)
            yield return null;

        _transform.parent = _parent;
        Return?.Invoke();
    }
}
