using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    private Rigidbody _rb;
    private float _damage = 4;
    private Pool<Bullet> _pool;
    private float _velocity;
    private Vector3 _lastVelocity;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _lastVelocity = _rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            Vector3 direction = Vector3.Reflect(_lastVelocity.normalized, collision.contacts[0].normal);
            _rb.velocity = direction * Mathf.Max(_lastVelocity.magnitude, 0f);
        }
    }

    public float GetDamage() => _damage;
    public void SetDirection(Vector3 direction) => _rb.velocity = direction * _velocity;
    public void SetVelocity(float velocity) => _velocity = velocity;
    public void SetPool(Pool<Bullet> pool) => _pool = pool;
    public void ReturnInPool() => _pool.Return(this);
    public void SetDamage(float damge) => _damage = damge;
}
