using Audio;
using Scriptable.Pooling;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

namespace Scriptable
{
    [CreateAssetMenu(fileName = "POOL_New_Sound_Emitter_Pool", menuName = "Pools/Sound Emitter Pool", order = 0)]
    public class SoundEmitterPoolSo : GameObjectPoolSo
    {
        [SerializeField] private GameObject _soundEmitterPrefab;
        
        public override GameObject Create()
        {
            var obj = Instantiate(original: _soundEmitterPrefab, parent: _poolRoot);
            obj.name = "SoundEmitter";
            return obj;
        }

        public override void Initialize()
        {
            if (HasBeenInitialized) return;
            Pool = new ObjectPool<GameObject>(createFunc: Create,
                actionOnGet: (obj) => obj.SetActive(true),
                actionOnRelease: (obj) => obj.SetActive(false),
                actionOnDestroy: (obj) => DestroyImmediate(obj),
                collectionCheck: false,
                defaultCapacity: DefaultSize,
                maxSize: MaxSize);
            HasBeenInitialized = true;
        }
    }
}