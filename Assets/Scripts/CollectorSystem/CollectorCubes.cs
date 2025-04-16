using System;
using UnityEngine;

public class CollectorCubes : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;

    public event Action<double> PutCoins;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Cube>(out Cube cube))
        {
            _wallet.PutCoins(cube.Cost);
            PutCoins?.Invoke(cube.Cost);
            cube.TakeDamage(cube.Hilth);
            Destroy(cube.gameObject);
        }
    }
}
