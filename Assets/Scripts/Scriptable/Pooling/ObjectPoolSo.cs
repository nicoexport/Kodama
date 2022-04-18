using System;
using Pooling;
using UnityEngine;
using UnityEngine.Pool;

namespace Scriptable.Pooling
{
    public abstract class ObjectPoolSo<T> : ScriptableObject, IPool<T> where T : class
    {
        protected ObjectPool<T> Pool;
        protected bool HasBeenInitialized { get; set; }
        private protected Transform _parent;
        private Transform _poolRoot;
        protected Transform PoolRoot
        {
            get
            {
                if (!_poolRoot)
                {
                    _poolRoot = new GameObject(name).transform;
                    _poolRoot.SetParent(_parent);
                }
                return _poolRoot;
            }
        }

        public abstract T Create();

        public abstract void Initialize();
        

        public virtual void OnDisable()
        {
            Pool.Clear();
        }

        public void SetParent(Transform t)
        {
            _parent = t;
            PoolRoot.SetParent(t);
        }
    }
}