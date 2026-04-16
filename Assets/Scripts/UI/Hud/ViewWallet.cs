using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ViewWallet : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private Animator _animation;
    
    private string _nameParametrs = "Fail";

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

    private void ChangeText(float count)
    {
        _text.text = NumberFormatter.Format(count);
    }

    private void StartAnimateFail()
    {
        _animation.SetBool(_nameParametrs, true);
    }
}
