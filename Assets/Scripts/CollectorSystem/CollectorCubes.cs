using System;
using UnityEngine;

public class CollectorCubes : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private SplashGold _splashGold;
 
    public event Action<float> PutCoins;
    public event Action<bool> ColectCore;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Cube>(out Cube cube))
        {
            if (cube.IsColect)
                return;

            if (cube.TryGetComponent<CoreCube>(out CoreCube core))
                ColectCore?.Invoke(core.IsTsar);

            if (cube.IsTsar == false)
                PutCoins?.Invoke(cube.Cost);

            _wallet.PutCoins(cube.Cost);
            cube.IsColect = true;
            cube.StartDastroy();

            _splashGold.Splash(cube.transform.position.x);
        }
    }
}
