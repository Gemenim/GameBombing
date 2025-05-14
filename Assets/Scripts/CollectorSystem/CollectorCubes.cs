using System;
using UnityEngine;

public class CollectorCubes : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;

    public event Action<double> PutCoins;
    public event Action ColectCore;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Cube>(out Cube cube))
        {
            if (cube.TryGetComponent<CoreCube>(out CoreCube core))
                ColectCore?.Invoke();

            _wallet.PutCoins(cube.Cost);
            PutCoins?.Invoke(cube.Cost);
            cube.TakeDamage(cube.Hilth);
            Destroy(cube.gameObject);
        }
    }
}
