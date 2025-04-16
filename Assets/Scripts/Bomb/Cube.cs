using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] protected const int _defoltCost = 5;
    [SerializeField] protected const float _defoltHilth = 10;

    protected const float _levelCoefficient = 0.5f;

    protected int _level;
    protected bool _detouched;
    protected float _hilth;
    protected double _cost;

    public float Hilth => _hilth;
    public int Id { get; set; }
    public double Cost => _cost;

    public virtual void SetLevel(int level)
    {
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
           Detouch();
        }
    }

    [ContextMenu("Detouched")]
    protected void Detouch()
    {
        if (_detouched)
            return;

        _detouched = true;
        GetComponentInParent<Chip>().DetouchCubeRecalculate(this);
    }
}
