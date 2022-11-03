using Kodama.GameManagement;
using UnityEngine;

namespace Kodama.Architecture {
    public abstract class Resettable : MonoBehaviour {
        [SerializeField] private ResettableRuntimeSet _resettableRuntimeSet;

        protected virtual void OnEnable() => _resettableRuntimeSet.AddToList(this);

        protected virtual void OnDisable() => _resettableRuntimeSet.RemoveFromList(this);

        public abstract void OnLevelReset();
    }
}