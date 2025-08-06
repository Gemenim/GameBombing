using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] protected float _levelCoefficientHilth = 2.1f;
    [SerializeField] protected float _levelCoefficientCost = 1.75f;

    protected const float c_defoltCost = 10f;
    protected const float c_defoltHilth = 3f;

    protected Transform _transform;
    protected float _dalayToDestruction = 2.5f;
    protected int _level;
    protected bool _detouched;

    public bool IsColect = false;

    public float Hilth { get; protected set; }
    public int Id { get; set; }
    public float Cost { get; protected set; }
    public bool IsTsar { get; protected set; }

    protected virtual void Awake()
    {
        _transform = transform;
    }

    public virtual void SetSetings(int level, bool isTsar)
    {
        IsTsar = isTsar;
        int randomLevel = level + Random.Range(-2, 2);
        _level = randomLevel > 0 ? randomLevel : 1;
    }

    public virtual void CalculateStats()
    {
        Hilth = (c_defoltHilth * Mathf.Pow(_level, _levelCoefficientHilth) - (c_defoltHilth * _level));
        Cost = (c_defoltCost * Mathf.Pow(_level, _levelCoefficientCost) - (c_defoltCost * _level));

        if (Hilth == 0)
            Hilth = c_defoltHilth;

        if (Cost == 0)
            Cost = c_defoltCost;
    }

    public void TakeDamage(float damage)
    {
        Hilth -= damage;

        if (Hilth <= 0)
        {
            Hilth = 0;
            gameObject.layer = 10;
            Detouch();
        }
    }

    public virtual void StartDastroy()
    {
        StartCoroutine(Destruction());
    }

    [ContextMenu("Detouched")]
    private void Detouch()
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
