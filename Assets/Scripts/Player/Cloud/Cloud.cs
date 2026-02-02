using System.Collections;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] private BoxCollider[] _clouds;
    [SerializeField] private float _defoltDamage = 0.5f;
    [SerializeField] private float _dalay = 1f;

    private float _damage;
    private bool _isRady = true;

    private void Update()
    {
        if (_isRady)
        {
            int id = Random.Range(0, _clouds.Length);
            BoxCollider cloud = _clouds[id];
        }
    }

    private IEnumerator Shock()
    {
        WaitForSeconds dalay = new WaitForSeconds(_dalay);

        yield return dalay;

        _isRady = true;
    }
}
