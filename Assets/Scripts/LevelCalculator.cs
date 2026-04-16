using UnityEngine;

public static class LevelCalculator
{
    public static float Calculat(float defoltValue, float coefficien, int level)
    {
        float value = defoltValue * Mathf.Pow(level, coefficien);

        return value;
    }
    
    public static float Calculat(float defoltValue, float coefficien, int level, int levelMultiplicity, float coefficienLevelMultiplicity)
    {
        if (level % levelMultiplicity == 0)
        {
            int multiplier = level / levelMultiplicity;
            coefficien += coefficienLevelMultiplicity * multiplier;
        }

        float value = defoltValue * Mathf.Pow(coefficien, level);

        return value;
    }
}