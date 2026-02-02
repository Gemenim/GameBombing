using UnityEngine;

public class UpdaterLevelSkills : MonoBehaviour
{
    private const float c_levelCostCoefficient = 1.5f;

    [SerializeField] private UpdaterLevelUpgrad _updateLevelUpgrad;
    [SerializeField] private Ability[] _skills;
    [SerializeField] private float _startCostShot = 1;

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

        CalculateCost();
        _updateLevelUpgrad.ChangeText(Level, Cost);
    }

    private void CalculateCost()
    {
        Cost = LevelCalculator.Calculat(_startCostShot, c_levelCostCoefficient, Level);
    }
}
