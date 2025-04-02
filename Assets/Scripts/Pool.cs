using System;
using System.Collections.Generic;

public class Pool<T>
{
    private readonly Func<T> _preload;
    private readonly Action<T> _getAction;
    private readonly Action<T> _returnAction;
    private Queue<T> _pool = new Queue<T>();
    private List<T> _active = new List<T>();
    private int _count = 0;

    public event Action<int> ChangedCount;
    public event Action<int> ChangedCountActive;
    public event Action<int> ChangedCountCreat;

    public Pool(Func<T> preload, Action<T> getAction, Action<T> returnAction)
    {
        _preload = preload;
        _getAction = getAction;
        _returnAction = returnAction;
    }

    public T Get()
    {
        T item = _pool.Count > 0 ? _pool.Dequeue() : _preload();
        _getAction(item);
        _active.Add(item);
        _count++;

        ChangedCount?.Invoke(_count);
        ChangedCountActive?.Invoke(_active.Count);
        ChangedCountCreat?.Invoke(_active.Count + _pool.Count);

        return item;
    }

    public void Return(T item)
    {
        _returnAction(item);
        _pool.Enqueue(item);
        _active.Remove(item);
        ChangedCountActive?.Invoke(_active.Count);
    }
}
