using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ColorCube))]
public class Cube : MonoBehaviour
{
    protected const float c_defoltCost = 10f;
    protected const float c_defoltHilth = 2f;
    protected const float c_ñoefficientLevel = 0.05f;
    protected const float c_ñoefficientTsar = 1.25f;
    protected const int c_multiplierLevel = 5;

    [SerializeField] private GameObject _trail;

    protected Transform _transform;
    protected float _hilth;
    protected float _dalayToDestruction = 1f;
    protected int _level;
    protected float _levelCoefficientHilth = 2f;
    protected float _levelCoefficientCost = 1.5f;

    private ColorCube _colorCube;
    private Color _color;
    private float _destroyedColor = 0.5f;

    public bool IsColect = false;

    public int Id { get; set; }
    public float Cost { get; protected set; }
    public bool IsTsar { get; protected set; }
    public bool IsDetouch { get; private set; } = false;

    protected void Awake()
    {
        _transform = transform;
        _colorCube = GetComponent<ColorCube>();
        _color = _colorCube.Color;
    }

    public void SetSetings(int level, bool isTsar)
    {
        if (level > 1)
        {
            IsTsar = isTsar;
            int randomLevel = level + Random.Range(-1, 1);
            _level = randomLevel > 0 ? randomLevel : 1;
        }
        else
        {
            IsTsar = isTsar;
            _level = 1;
        }

        CalculateStats();
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

    private void CalculateStats()
    {
        if (IsTsar)
        {
            _hilth = LevelCalculator.Calculat(c_defoltHilth, _levelCoefficientHilth * c_ñoefficientTsar, _level, c_multiplierLevel, c_ñoefficientLevel);
            Cost = LevelCalculator.Calculat(c_defoltCost, _levelCoefficientCost * c_ñoefficientTsar, _level);
        }
        else
        {
            _hilth = LevelCalculator.Calculat(c_defoltHilth, _levelCoefficientHilth, _level, c_multiplierLevel, c_ñoefficientLevel);
            Cost = LevelCalculator.Calculat(c_defoltCost, _levelCoefficientCost, _level);
        }

        if (_hilth <= 0)
            _hilth = c_defoltHilth;

        if (Cost <= 0)
            Cost = c_defoltCost;
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
