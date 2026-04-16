using UnityEngine;

public class UpdaterLevelSkills : MonoBehaviour
{
    private const float c_levelCostCoefficient = 1.3f;

    [SerializeField] private UpdaterLevelUpgrad _updateLevelUpgrad;
    [SerializeField] private Ability[] _skills;
    [SerializeField] private float _defoltCostShot = 1f;

    private float _minCost = 1.5f;

    public int Level { get; private set; } = 1;
    public float Cost { get; private set; } = 0;

    private void OnEnable()
    {
        foreach (Ability ability in _skills)
            ability.LevelRaised += UpdateLevel;
    }

    private void OnDisable()
    {
        foreach (Ability ability in _skills)
            ability.LevelRaised -= UpdateLevel;
    }

    private void UpdateLevel(int level)
    {
        if (level > 1)
            Level++;

        Cost = LevelCalculator.Calculat(_defoltCostShot, c_levelCostCoefficient, Level);
        Debug.Log(Cost);
        Cost = Cost > _minCost ? Cost : 0;
        _updateLevelUpgrad.ChangeText(Level, Cost);
    }
}
