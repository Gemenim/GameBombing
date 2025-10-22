using UnityEngine;

public static class LevelCalculator
{
    public static float Calculat(float defoltValue, float coefficien, int level)
    {
        float value = defoltValue * Mathf.Pow(level, coefficien) - (defoltValue * level);

        return value;
    }
    
    public static float Calculat(float defoltValue, float coefficien, int level, int levelMultiplicity, float coefficienLevel)
    {
        if (level % levelMultiplicity == 0)
        {
            int multiplier = level / levelMultiplicity;
            coefficien += coefficienLevel * multiplier;
        }

        float value = defoltValue * Mathf.Pow(level, coefficien) - (defoltValue * level);

        return value;
    }
}
