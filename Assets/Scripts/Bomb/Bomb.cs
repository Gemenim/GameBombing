using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    [SerializeField] private CoreCube _core;

    private Transform _transform;
    private Rigidbody _rb;
    private int[,] _cubesInfo;
    private Vector3 _cubesInfoStartPosition;
    private Cube[] _cubes;

    public event Action Destroyed;

    private void Awake()
    {
        _transform = transform;
        _rb = GetComponent<Rigidbody>();
        _rb.mass = transform.childCount;
        CollectCubes();
        RecalculateCubes();
    }

    private void Start()
    {
        SetStats();        
    }

    private void OnEnable()
    {
        _core.Destroyed += Destroy;
    }

    private void OnDisable()
    {
        _core.Destroyed -= Destroy;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Bullet>(out Bullet bullet))
        {
            Cube cube = collision.GetContact(0).thisCollider.GetComponent<Cube>();
            cube.TakeDamage(bullet.GetDamage());
        }
    }

    public void SetLevel(int level) => _core.SetLevel(level);

    public void DetouchCubeRecalculate(Cube cube)
    {
        Vector2Int grid = GridPosition(cube.transform.localPosition);
        _cubesInfo[grid.x, grid.y] = 0;
        _cubes[cube.Id - 1] = null;

        cube.transform.parent = null;
        Rigidbody rb = cube.gameObject.AddComponent<Rigidbody>();

        RecalculateCubes();
    }

    private void DetouchCube(Cube cube)
    {
        Vector2Int grid = GridPosition(cube.transform.localPosition);
        _cubesInfo[grid.x, grid.y] = 0;
        _cubes[cube.Id - 1] = null;

        cube.transform.parent = null;
        Rigidbody rb = cube.gameObject.AddComponent<Rigidbody>();
    }

    private void CollectCubes()
    {
        Vector3 min = Vector3.one * float.MaxValue;
        Vector3 max = Vector3.one * float.MinValue;

        for (int i = 0; i < _transform.childCount; i++)
        {
            Transform child = _transform.GetChild(i);
            min = Vector3.Min(min, child.localPosition);
            max = Vector3.Max(max, child.localPosition);
        }

        Vector2Int delta = Vector2Int.RoundToInt(max - min);
        _cubesInfo = new int[delta.x + 1, delta.y + 1];
        _cubesInfoStartPosition = min;
        _cubes = GetComponentsInChildren<Cube>();

        for (int i = 0; i < _transform.childCount; i++)
        {
            Transform child = _transform.GetChild(i);
            Vector2Int grid = GridPosition(child.localPosition);
            _cubesInfo[grid.x, grid.y] = i + 1;
            _cubes[i].Id = i + 1;
        }
    }

    private void RecalculateCubes()
    {
        List<int> freeCubesIds = new List<int>();

        for (int i = 0; i < _cubes.Length; i++)
        {
            if (_cubes[i] != null)
                freeCubesIds.Add(_cubes[i].Id);
        }

        if (freeCubesIds.Count == 0)
        {
            Destroy(gameObject);
            return;
        }

        List<CubeGroup> groups = new List<CubeGroup>();
        int currentGroup = 0;

        while (freeCubesIds.Count > 0)
        {
            groups.Add(new CubeGroup());
            int id = freeCubesIds[0];
            groups[currentGroup].Cubes.Add(id);
            freeCubesIds.Remove(id);
            checkCube(id);
            currentGroup++;

            void checkCube(int id)
            {
                Vector2Int gridPosition = GridPosition(_cubes[id - 1].transform.localPosition);

                checkNeighbor(Vector2Int.up);
                checkNeighbor(Vector2Int.down);
                checkNeighbor(Vector2Int.left);
                checkNeighbor(Vector2Int.right);

                void checkNeighbor(Vector2Int direction)
                {
                    int id = GetNeighbor(gridPosition, direction);

                    if (freeCubesIds.Remove(id))
                    {
                        groups[currentGroup].Cubes.Add(id);
                        checkCube(id);
                    }
                }
            }
        }

        if (groups.Count < 2)
            return;

        for (int i = 1; i < groups.Count; i++)
        {
            GameObject bomb = new GameObject("Bomb");
            Transform firstCube = _cubes[groups[i].Cubes[0] - 1].transform;
            bomb.transform.SetPositionAndRotation(firstCube.position, firstCube.rotation);

            foreach (int id in groups[i].Cubes)
                _cubes[id - 1].transform.parent = bomb.transform;

            bomb.AddComponent<Bomb>().SetCore(_core);
        }

        CollectCubes();
    }

    private void Destroy()
    {
        foreach (Cube cube in _cubes)
        {
            if (cube != null)
                DetouchCube(cube);
        }

        RecalculateCubes();

        Destroyed?.Invoke();
    }

    private Vector2Int GridPosition(Vector3 localPosition)
    {
        return Vector2Int.RoundToInt(localPosition - _cubesInfoStartPosition);
    }

    private int GetNeighbor(Vector2Int position, Vector2Int direction)
    {
        Vector2Int gridPosition = position + direction;

        if (gridPosition.x < 0 || gridPosition.x >= _cubesInfo.GetLength(0)
            || gridPosition.y < 0 || gridPosition.y >= _cubesInfo.GetLength(1))
            return 0;

        return _cubesInfo[gridPosition.x, gridPosition.y];
    }

    private void SetCore(CoreCube core)
    {
        _core = core;
    }

    private void SetStats()
    {
        foreach (Cube cube in _cubes)
        {
            if (cube.name != _core.name)
            {
                cube.SetLevel(_core.Level);
            }

            cube.CalculateStats();
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        Gizmos.matrix = _transform.localToWorldMatrix;

        for (int x = 0; x < _cubesInfo.GetLength(0); x++)
        {
            for (int y = 0; y < _cubesInfo.GetLength(1); y++)
            {
                Vector3 position = _cubesInfoStartPosition + new Vector3(x, y, 0);

                if (_cubesInfo[x, y] == 0)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(position, 0.1f);
                }
                else
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(position, 0.2f);
                }
            }
        }
    }
}

public class CubeGroup
{
    public List<int> Cubes = new List<int>();
}