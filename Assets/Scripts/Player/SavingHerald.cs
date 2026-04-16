using System;
using System.Collections;
using UnityEngine;
using YG;

public class SavingHerald : MonoBehaviour
{
    [SerializeField] private MessageBox _messageBox;

    private Coroutine _corutine = null;

    public event Action Saved;
    public event Action Uploaded;

    public void Load() => Uploaded?.Invoke();
    public void Save()
    {
        Saved?.Invoke();

        if (_corutine == null)
        {
            _corutine = StartCoroutine(SaveData());
        }
        else
        {
            StopCoroutine(_corutine);
            _corutine = StartCoroutine(SaveData());
        }
    }

    private IEnumerator SaveData()
    {
        yield return null;

        YandexGame.SaveProgress();
    }
}
