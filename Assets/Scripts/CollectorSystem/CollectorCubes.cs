using System;
using UnityEngine;

public class CollectorCubes : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;

    public event Action<double> PutCoins;
    public event Action<bool> ColectCore;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Cube>(out Cube cube))
        {
            if (cube.TryGetComponent<CoreCube>(out CoreCube core))
                ColectCore?.Invoke(core.IsTsar);

            if (cube.IsTsar == false)
                PutCoins?.Invoke(cube.Cost);

            _wallet.PutCoins(cube.Cost);
            cube.TakeDamage(cube.Hilth);
            cube.StartDastroy();
        }
    }
}
