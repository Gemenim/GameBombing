using System;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    [SerializeField] protected int _maxLevel;
    [SerializeField] public string Name;

    public int Level { get; private set; } = 1;

    public event Action LevelLimit;
    public event Action<int> LevelRaised;

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

    public void Load(int level)
    {
        Level = level;
        LevelRaised?.Invoke(Level);

        if (Level >= _maxLevel)
        {
            Level = _maxLevel;
            LevelLimit?.Invoke();
        }

        UpdateStatus();

        Debug.Log(Name + Level);
    }

    protected abstract void UpdateStatus();
}
