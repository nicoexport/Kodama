using System;
using Scriptable;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioCueChannelSo _sfxChannel;
        [SerializeField] private AudioCueChannelSo _musicChannel;

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