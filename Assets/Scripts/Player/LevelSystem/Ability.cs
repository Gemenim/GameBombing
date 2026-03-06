using System;
using UnityEngine;
using YG;

public abstract class Ability : MonoBehaviour
{
    [SerializeField] private SavingHerald _savingHerald;
    [SerializeField] protected BulletObject _bulletObject;
    [SerializeField] protected int _maxLevel;
    [SerializeField] public string Name;
    [SerializeField] public int ID;

    public int Level { get; private set; } = 1;

    public event Action LevelLimit;
    public event Action<int> LevelRaised;

    private void OnEnable()
    {
        _savingHerald.Saved += Save;
        _savingHerald.Uploaded += Load;
    }

    private void OnDisable()
    {
        _savingHerald.Saved -= Save;
        _savingHerald.Uploaded -= Load;
    }

    public void UpLevel()
    {
        if (Level < _maxLevel)
        {
            Level++;
            LevelRaised?.Invoke(Level);
            UpdateStatus();
        }
        else
        {
            LevelLimit?.Invoke();
            return;
        }
    }

    private void Save() => YandexGame.savesData.Levels[ID] = Level;

    public void Load()
    {
        Level = YandexGame.savesData.Levels[ID];
        LevelRaised?.Invoke(Level);

        if (Level >= _maxLevel)
        {
            Level = _maxLevel;
            LevelLimit?.Invoke();
        }

        UpdateStatus();
    }

    protected abstract void UpdateStatus();
}
