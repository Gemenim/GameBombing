using TMPro;
using UnityEngine;

public class UpdateLevelUpgrad : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private TextMeshProUGUI _cost;

    public void ChangeText(int level, float cost)
    {
        _level.text = level.ToString();
        _cost.text = NumberFormatter.Format(cost);
    }
}
