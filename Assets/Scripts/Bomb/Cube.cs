using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] protected float _levelCoefficientHilth = 2.1f;
    [SerializeField] protected float _levelCoefficientCost = 2.5f;

    protected const double _defoltCost = 1;
    protected const float _defoltHilth = 10;

    protected Transform _transform;
    protected float _dalayToDestruction = 2.5f;
    protected int _level;
    protected bool _detouched;

    public bool IsColect = false;

    public float Hilth { get; protected set; }
    public int Id { get; set; }
    public double Cost { get; protected set; }
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
        Hilth = (_defoltHilth * Mathf.Pow(_level, _levelCoefficientHilth) + (_defoltHilth * _level));
        Cost = (_defoltCost * Mathf.Pow(_level, _levelCoefficientCost) +(_defoltCost * _level));
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
