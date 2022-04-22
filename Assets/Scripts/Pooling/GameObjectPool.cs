using System;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Pooling
{
    public class GameObjectPool : IDisposable
    {
        private readonly IObjectPool<GameObject> _pool;

        public GameObjectPool(GameObject gameObject, Transform parent, int defaultSize, int maxSize)
        {
            _pool = new ObjectPool<GameObject>(
                createFunc: () => Object.Instantiate(gameObject, parent),
                actionOnGet: (obj) => obj.SetActive(true),
                actionOnRelease: (obj) => obj.SetActive(false),
                actionOnDestroy: Object.Destroy,
                collectionCheck: false,
                defaultCapacity: defaultSize,
                maxSize: maxSize
            );
        }
        
        public GameObject Get()
        {
            return _pool.Get();
        }

        public void Release(GameObject gameObject)
        {
            _pool.Release(gameObject);
        }

        public void Clear()
        {
            _pool.Clear();
        }

        public void Dispose()
        {
            Clear();
        }
    }
}