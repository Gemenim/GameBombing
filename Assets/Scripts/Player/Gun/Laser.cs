using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour
{
    [SerializeField] private int _countReflections = 1;
    [SerializeField] private float _maxDistace = 30f;
    [SerializeField] private int _layerMask;

    private Transform _transform;
    private LineRenderer _lineRenderer;
    private Ray _ray;
    private RaycastHit _hit;
    private float _positionZ;

    private void OnValidate()
    {
        if (_countReflections < 0)
            _countReflections = 1;
    }

    private void Awake()
    {
        _transform = transform;
        _lineRenderer = GetComponent<LineRenderer>();
        _positionZ = _transform.position.z;
    }

    private void Update()
    {
        _ray = new Ray(_transform.localPosition, _transform.up);

        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, _transform.localPosition);
        float remainingLength = _maxDistace;

        for (int i = 0; i < _countReflections; i++)
        {
            if (Physics.Raycast(_ray.origin, _ray.direction, out _hit, remainingLength))
            {
                _lineRenderer.positionCount += 1;
                _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _hit.point);
                remainingLength -= Vector3.Distance(_ray.origin, _hit.point);
                Vector3 direction = Vector3.Reflect(_ray.direction, _hit.normal);
                direction.z = _positionZ;
                _ray = new Ray(_hit.point, direction);
            }
            else
            {
                _lineRenderer.positionCount += 1;
                _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, _ray.origin + _ray.direction * remainingLength);
            }
        }
    }
}