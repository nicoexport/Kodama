using System;
using System.Runtime.Serialization;
using Scriptable;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioCueChannelSo _sfxChannel;
        [SerializeField] private AudioCueChannelSo _musicChannel;

        [SerializeField] private GameObject _soundEmitterPrefab;

        private ObjectPool<GameObject> _pool;
        
        private void Awake()
        {
            _pool = new ObjectPool<GameObject>(createFunc: () => CreatePoolObject(), 
                actionOnGet: (obj) => obj.SetActive(true), 
                actionOnRelease: (obj) => obj.SetActive(false),
                actionOnDestroy: (obj) => Destroy(obj),
                collectionCheck: false, 
                defaultCapacity: 10,
                maxSize: 10);
            _pool.Get();
        }

        private GameObject CreatePoolObject()
        {
            var obj = Instantiate(original: _soundEmitterPrefab, parent: this.transform);
            obj.name = "SoundEmitter";
            return obj;
        }

        private void OnEnable()
        {
            _sfxChannel.OnAudioCueRequested += PlayAudio;
            _musicChannel.OnAudioCueRequested += PlayAudio;
        }

        private void OnDisable()
        {
            _sfxChannel.OnAudioCueRequested -= PlayAudio;
            _musicChannel.OnAudioCueRequested -= PlayAudio;
        }

        private void PlayAudio(AudioCueRequestData audioCueRequestData)
        {
            
        }
    }
}