using UnityEngine;
using UnityEngine.Audio;

public class TotalVolumeChanged : MonoBehaviour
{
    private const string c_name = "Master";
    private const float c_startVolume = 0.7f;

    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private MuteSourceButton _mute;

    private void Start()
    {
        Change(c_startVolume);
    }

    public void Change(float value)
    {
        _mixer.SetFloat(c_name, Mathf.Lerp(-80, 0, value));

        if (value == 0)
        {
            _mute.SetMute(true);
        }
        else
        {
            if (_mute.IsMute)
                _mute.SetMute(false);
        }
    }
}
