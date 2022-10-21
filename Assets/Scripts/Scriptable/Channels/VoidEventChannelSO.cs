using System;
using UnityEngine;

namespace Scriptable.Channels
{
    [CreateAssetMenu(menuName = "Channels/Void Event")]
    public class VoidEventChannelSO : ScriptableObject
    {
        public event Action OnEventRaised;

        public void RaiseEvent()
        {
            OnEventRaised?.Invoke();
        }
    }
}