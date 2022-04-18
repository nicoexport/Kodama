using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Scriptable.Pooling
{
    public abstract class GameObjectPoolSo : ObjectPoolSo<GameObject>
    {
        private Transform _parent;
        private protected Transform _poolRoot;

        private Transform PoolRoot
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
        
        public void SetParent(Transform t)
        {
            _parent = t;
            PoolRoot.SetParent(t);
        }
        
        public abstract override GameObject Create();
        public abstract override void Initialize();

    }
}