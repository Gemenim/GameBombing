using TMPro;
using UnityEngine;

public class MessageBox : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private TextMeshProUGUI[] _texts;

    public bool IsOn { get; private set; } = false;

    public void Switch(int i)
    {
        IsOn = !IsOn;
        ShowText(i);
        _animator.SetBool("IsOn", IsOn);
    }

    private void ShowText(int i)
    {
        foreach (TextMeshProUGUI text in _texts)
            text.gameObject.SetActive(false);

        _texts[i].gameObject.SetActive(true);
    }
}
