using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Pooling
{
    public class GameObjectPool : IDisposable
    {
        private readonly IObjectPool<GameObject> _pool;
        private readonly Transform _parent;
        private readonly GameObject _prefab;
        public readonly int capacity;
        public event Action<GameObject> onInstantiate;
        public GameObjectPool(Transform parent, GameObject prefab, int capacity)
        {
            Assert.IsTrue(parent);
            this._prefab = prefab;
            this._parent = parent;
            this.capacity = capacity;
            _pool = new LinkedPool<GameObject>(
                createFunc: InstantiateInstance,
                actionOnGet: EnableInstance,
                actionOnRelease: DisableInstance,
                actionOnDestroy: DestroyInstance,
                collectionCheck: false,
                maxSize: capacity
            );
        }
        
        public GameObjectPool(Transform parent, int capacity)
        {
            Assert.IsTrue(parent);
            this._parent = parent;
            this.capacity = capacity;
            _pool = new LinkedPool<GameObject>(
                createFunc: CreateInstance,
                actionOnGet: EnableInstance,
                actionOnRelease: DisableInstance,
                actionOnDestroy: DestroyInstance,
                collectionCheck: false,
                maxSize: capacity
            );
        }
        
        private void DestroyInstance(GameObject obj)
        {
            Object.Destroy(obj);
        }

        private void DisableInstance(GameObject obj)
        {
            obj.SetActive(false);
        }

        private void EnableInstance(GameObject obj)
        {
            obj.SetActive(true);
        }

        private GameObject InstantiateInstance()
        {
            var obj = Object.Instantiate(_prefab, _parent);
            onInstantiate?.Invoke(obj);
            return obj;
        }

        private GameObject CreateInstance()
        {
            var obj = new GameObject();
            obj.transform.parent = _parent;
            onInstantiate?.Invoke(obj);
            return obj;
        }
        
        public GameObject Request()
        {
            return _pool.Get();
        }

        public void Return(GameObject obj)
        {
            _pool.Release(obj);
        }

        public void Dispose()
        {
            _pool.Clear();
        }
    }
}