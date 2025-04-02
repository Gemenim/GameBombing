using TMPro;
using UnityEngine;

public class ViewWallet : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Wallet _wallet;

    private string[] _nomes = new[] { "", "K", "M", "B" };

    private void OnEnable()
    {
        _wallet.ChangeCount += ChangeText;
    }

    private void OnDisable()
    {
        _wallet.ChangeCount -= ChangeText;
    }

    private void ChangeText(double count)
    {
        _text.text = FormatNam(count);
    }

    private string FormatNam(double num)
    {
        if (num == 0)
            return "0";

        num = Mathf.Round((float)num);

        int i = 0;

        while (i + 1 < _nomes.Length && num >= 1000d)
        {
            num /= 1000d;
            i++;
        }

        return num.ToString( format: "#,##") + _nomes[i];
    }
}
