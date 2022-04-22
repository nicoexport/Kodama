using System;
using System.Runtime.Serialization;
using Pooling;
using Scriptable;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioCueChannelSo _sfxChannel;
        [SerializeField] private AudioCueChannelSo _musicChannel;
        [SerializeField] private GameObject _soundEmitterPrefab;
        private GameObjectPool _soundEmitterPool;

        private void Awake()
        {
            _soundEmitterPool = new GameObjectPool(_soundEmitterPrefab, this.transform, 12, 12);
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
            AudioClip[] clipsToPlay = audioCueRequestData.AudioCue.GetClips();
            int numberOfClips = clipsToPlay.Length;

            for (int i = 0; i < numberOfClips; i++)
            {
                SoundEmitter soundEmitter = _soundEmitterPool.Get().GetComponent<SoundEmitter>();
                
                if (soundEmitter == null) return;
                soundEmitter.PlayAudioClip(clipsToPlay[i], audioCueRequestData.AudioConfig,
                    audioCueRequestData.AudioCue.Looping, audioCueRequestData.Position);
                
                if (!audioCueRequestData.AudioCue.Looping)
                    soundEmitter.OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
            }
        }

        private void OnSoundEmitterFinishedPlaying(SoundEmitter soundEmitter)
        {
            soundEmitter.OnSoundFinishedPlaying -= OnSoundEmitterFinishedPlaying;
            soundEmitter.Stop();
            _soundEmitterPool.Release(soundEmitter.gameObject);
        }
    }
}