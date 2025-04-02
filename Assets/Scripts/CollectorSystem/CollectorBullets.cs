using UnityEngine;

public class CollectorBullets : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Bullet>(out Bullet bullet))
            bullet.ReturnInPool();
    }
}