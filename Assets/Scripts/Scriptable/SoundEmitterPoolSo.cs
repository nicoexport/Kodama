using UnityEngine;
using UnityEngine.Pool;

namespace Scriptable
{
    [CreateAssetMenu(fileName = "POOL_New_Sound_Emitter_Pool", menuName = "Pools/Sound Emitter Pool", order = 0)]
    public class SoundEmitterPoolSo : ScriptableObject
    {
        private ObjectPool<GameObject> _pool;
        
        
    }
}