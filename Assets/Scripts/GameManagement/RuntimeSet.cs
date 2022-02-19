using UnityEngine;
using System.Collections.Generic;


public abstract class RuntimeSet<T> : ScriptableObject
{
    private List<T> _items = new List<T>();

    public void Initialize()
    {
        _items.Clear();
    }

    public T GetItemAtIndex(int index)
    {
        return _items[index];
    }

    public void AddToList(T thingToAdd)
    {
        if (_items.Contains(thingToAdd)) return;
        _items.Add(thingToAdd);
    }

    public void RemoveFromList(T thingToRemove)
    {
        if (!_items.Contains(thingToRemove)) return;
        _items.Remove(thingToRemove);
    }

    public List<T> GetItemList()
    {
        return _items;
    }
}