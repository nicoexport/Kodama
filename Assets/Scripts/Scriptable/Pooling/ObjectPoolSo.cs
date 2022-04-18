using System;
using Pooling;
using UnityEngine;
using UnityEngine.Pool;

namespace Scriptable.Pooling
{
    public abstract class ObjectPoolSo<T> : ScriptableObject, IPool<T> where T : class
    {
        public IObjectPool<T> Pool;
        protected bool HasBeenInitialized { get; set; }
        [SerializeField] protected int DefaultSize;
        [SerializeField] protected int MaxSize;
        
        public abstract T Create();
        public abstract void Initialize();
        public virtual void OnDisable()
        {
            HasBeenInitialized = false;
            Pool?.Clear();
        }
    }
}