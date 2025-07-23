using UnityEngine;
using UnityEngine.UI;

public class MuteSourceButton : MonoBehaviour
{
    [SerializeField] private AudioSource[] _audio;
    [SerializeField] private Image _image;
    [SerializeField] private Sprite _on, _off;

    public bool IsMute { get; private set; } = false;

    public void OnOffMute()
    {
        IsMute = !IsMute;

        foreach (AudioSource audio in _audio)
        {
            audio.mute = IsMute;
            ChangeImage();
        }
    }

    public void MuteWindow(bool isVisibility)
    {
        if (IsMute)
            return;

        if (isVisibility)
        {
            foreach (AudioSource audio in _audio)
                audio.mute = false;
        }
        else
        {
            foreach (AudioSource audio in _audio)
                audio.mute = true;
        }
    }

    public void SetMute(bool isMute)
    {
        if (isMute == IsMute)
            return;

        IsMute = isMute;

        foreach (AudioSource audio in _audio)
            audio.mute = IsMute;

        ChangeImage();
    }

    private void ChangeImage()
    {
        if (IsMute)
            _image.overrideSprite = _off;
        else
            _image.overrideSprite = _on;
    }
}
