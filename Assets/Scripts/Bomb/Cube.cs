using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] protected const int _defoltCost = 2;
    [SerializeField] protected const float _defoltHilth = 10;

    protected const float _levelCoefficient = 0.5f;

    protected Transform _transform;
    protected float _dalayToDestruction = 2.5f;
    protected int _level;
    protected bool _detouched;
    protected float _hilth;
    protected double _cost;
    protected bool _isTsar;


    public float Hilth => _hilth;
    public int Id { get; set; }
    public double Cost => _cost;
    public bool IsTsar => _isTsar;

    protected virtual void Awake()
    {
        _transform = transform;
    }

    public virtual void SetSetings(int level, bool isTsar)
    {
        _isTsar = isTsar;
        int randomLevel = level + Random.Range(-2, 2);
        _level = randomLevel > 0 ? randomLevel : 1;
    }

    public virtual void CalculateStats()
    {
        _hilth = _defoltHilth * _level * _levelCoefficient;
        _cost = _defoltCost + _defoltCost * _level;
    }

    public void TakeDamage(float damage)
    {
        _hilth -= damage;

        if (_hilth <= 0)
        {
            _hilth = 0;
            gameObject.layer = 10;
            Detouch();
        }
    }

    public virtual void StartDastroy()
    {
        StartCoroutine(Destruction());
    }

    [ContextMenu("Detouched")]
    protected void Detouch()
    {
        if (_detouched)
            return;

        _detouched = true;
        Chip chip = GetComponentInParent<Chip>();

        if (chip != null)
            chip.DetouchCubeRecalculate(this);
    }

    protected IEnumerator Destruction()
    {
        Debug.Log("start");
        float _dalay = 0;
        Vector3 startScale = _transform.localScale;
        Vector3 endScale = new Vector3(0, 0, 0);

        while (true)
        {
            _dalay += Time.deltaTime;
            _transform.localScale = Vector3.Lerp(startScale, endScale, _dalay / _dalayToDestruction);

            if (_dalay >= _dalayToDestruction)
                Destroy(this.gameObject);

            yield return null;
        }
    }
}
