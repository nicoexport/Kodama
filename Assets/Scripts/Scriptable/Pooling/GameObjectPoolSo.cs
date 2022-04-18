using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Scriptable.Pooling
{
    public class GameObjectPoolSo : ObjectPool<GameObject>
    {
        public GameObjectPoolSo(Func<GameObject> createFunc, Action<GameObject> actionOnGet = null, Action<GameObject> actionOnRelease = null, Action<GameObject> actionOnDestroy = null, bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 10000) : base(createFunc, actionOnGet, actionOnRelease, actionOnDestroy, collectionCheck, defaultCapacity, maxSize)
        {
        }
    }
}