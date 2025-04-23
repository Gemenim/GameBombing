using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ViewWallet : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private float _duration = 3;

    private string[] _nomes = new[] { "", "K", "M", "B" };
    private Image _image;
    private Color _startColor;
    private Color _targetColor;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _startColor = _image.color;
        _targetColor = new Color(1, 0, 0, _startColor.a);
    }

    private void OnEnable()
    {
        _wallet.ChangeCount += ChangeText;
        _wallet.Fail += StartAnimateFail;
    }

    private void OnDisable()
    {
        _wallet.ChangeCount -= ChangeText;
        _wallet.Fail -= StartAnimateFail;
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

        return num.ToString(format: "#,##") + _nomes[i];
    }

    private void StartAnimateFail()
    {
        StartCoroutine(AnimateFail());
    }

    private IEnumerator AnimateFail()
    {
        WaitForFixedUpdate dalay = new WaitForFixedUpdate();
        float time = 0;

        while (time < _duration)
        {
            _image.color = Color.Lerp(_startColor, _targetColor, Mathf.PingPong(Time.time, 1));
            time += Time.deltaTime;

            if (time >= _duration)
                _image.color = Color.Lerp(_image.color, _startColor, _duration);

            yield return dalay;
        }
    }
}
