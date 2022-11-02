using System;
using UnityEngine;

namespace Scriptable.Channels {
    public abstract class BaseEventChannelSO<T> : ScriptableObject {
        public event Action<T> OnEventRaised;

        public void RaiseEvent(T item) => OnEventRaised?.Invoke(item);
    }
}