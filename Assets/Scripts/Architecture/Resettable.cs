using GameManagement;
using UnityEngine;

namespace Architecture
{
    public abstract class Resettable : MonoBehaviour
    {
        [SerializeField] ResettableRuntimeSet _resettableRuntimeSet;

        protected void OnEnable()
        {
            _resettableRuntimeSet.AddToList(this);
        }

        protected void OnDisable()
        {
            _resettableRuntimeSet.RemoveFromList(this);
        }

        public abstract void ResetResettable();
    }
}