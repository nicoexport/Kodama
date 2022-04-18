using Audio;
using Scriptable.Pooling;
using UnityEngine;
using UnityEngine.Pool;

namespace Scriptable
{
    [CreateAssetMenu(fileName = "POOL_New_Sound_Emitter_Pool", menuName = "Pools/Sound Emitter Pool", order = 0)]
    public class SoundEmitterPoolSo : ObjectPoolSo<GameObject>
    {
        [SerializeField] private GameObject _soundEmitterPrefab;
        
        public override GameObject Create()
        {
            return Instantiate(_soundEmitterPrefab, _parent);
        }

        public override void Initialize()
        {
            Pool = new ObjectPool<GameObject>(createFunc: Create, 
                actionOnGet: (obj) => obj.SetActive(true), 
                actionOnRelease: (obj) => obj.SetActive(false),
                actionOnDestroy: (obj) => Destroy(obj),
                collectionCheck: false, 
                defaultCapacity: 10,
                maxSize: 10);
        }
    }
}