using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    [SerializeField] private MoverPlatform _moverPlatform;

    private List<Transform> _targets = new List<Transform>();
    private Transform _target = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Bullet>(out Bullet bullet))
        {
            Debug.Log(bullet.name);
            if (_target == null)
            {
                _target = bullet.transform;
                _moverPlatform.SetTarget(_target);
            }
            else
            {
                _targets.Add(_target);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Bullet>(out Bullet bullet))
        {
            if (_target == bullet.transform)
            {
                SetTarget();
            }
            else
            {
                _targets.Remove(bullet.transform);
            }
        }
    }

    private void Update()
    {
        if (_target != null)
        {
            if (!_target.gameObject.activeSelf)
            {
                SetTarget();
            }
        }
    }

    private void SetTarget()
    {
        if (_targets.Count > 0)
        {
            _moverPlatform.SetTarget(_targets[0]);
            _targets.RemoveAt(0);
        }
        else
        {
            _target = null;
            _moverPlatform.RetornEndPosint();
        }
    }
}
