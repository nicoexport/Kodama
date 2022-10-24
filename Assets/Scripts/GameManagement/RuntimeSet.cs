using System.Collections.Generic;
using UnityEngine;

namespace GameManagement
{
    public abstract class RuntimeSet<T> : ScriptableObject
    {
        private readonly List<T> _items = new();

        public void Initialize()
        {
            _items.Clear();
        }

        public T GetItemAtIndex(int index)
        {
            if (_items.Count == 0) return default;
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
        
        public bool TryGetFirst(out T item)
        {
            item = GetItemAtIndex(0);
            return item != null;
        }
        

        public bool IsEmpty()
        {
            if (_items.Count <= 0) return true;
            return false;
        }
    }
}