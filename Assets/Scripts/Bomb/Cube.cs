using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ColorCube))]
public class Cube : MonoBehaviour
{
    protected const float c_defoltCost = 10f;
    protected const float c_defoltHilth = 3f;
    protected const float c_ñoefficientLevel = 0.05f;
    protected const int c_multiplierLevel = 5;

    [SerializeField] private GameObject _trail;
    [Range(0, 50)]
    [SerializeField] private float _force = 0.1f;

    [SerializeField] protected float _levelCoefficientHilth = 2.1f;
    [SerializeField] protected float _levelCoefficientCost = 1.7f;

    protected Transform _transform;
    protected float _hilth;
    protected float _dalayToDestruction = 1f;
    protected int _level;

    private ColorCube _colorCube;
    private Color _color;
    private float _destroyedColor = 0.5f;

    public bool IsColect = false;

    public int Id { get; set; }
    public float Cost { get; protected set; }
    public bool IsTsar { get; protected set; }
    public bool IsDetouch { get; private set; } = false;

    protected virtual void Awake()
    {
        _transform = transform;
        _colorCube = GetComponent<ColorCube>();
        _color = _colorCube.Color;
    }

    public virtual void SetSetings(int level, bool isTsar)
    {
        IsTsar = isTsar;
        int randomLevel = level + Random.Range(-2, 2);
        _level = randomLevel > 0 ? randomLevel : 1;
    }

    public virtual void CalculateStats()
    {
        _hilth = LevelCalculator.Calculat(c_defoltHilth, _levelCoefficientHilth, _level, c_multiplierLevel, c_ñoefficientLevel);
        Cost = LevelCalculator.Calculat(c_defoltCost, _levelCoefficientCost, _level);

        if (_hilth == 0)
            _hilth = c_defoltHilth;

        if (Cost == 0)
            Cost = c_defoltCost;
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

    public virtual void StartDastroy()
    {
        CheckParent();
        StartCoroutine(Destruction());
    }

    [ContextMenu("Detouched")]
    public virtual void Detouch()
    {
        if (IsDetouch)
            return;

        IsDetouch = true;
        ChangeColor();
        CheckParent();

        if (!IsColect)
            _trail.SetActive(true);
    }

    private void CheckParent()
    {
        Chip chip = GetComponentInParent<Chip>();

        if (chip != null)
            chip.DetouchCubeRecalculate(this);
    }

    private void ChangeColor()
    {
        Color color = _color * _destroyedColor;
        _colorCube.ApplyColor(color);
    }

    protected IEnumerator Destruction()
    {
        float _dalay = 0;
        Vector3 startScale = _transform.localScale;
        Vector3 endScale = new Vector3(0, 0, 0);
        _trail.SetActive(false);

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
