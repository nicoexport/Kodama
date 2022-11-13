using System;
using Kodama.Scriptable;
using Kodama.Scriptable.Channels;
using Slothsoft.UnityExtensions;
using UnityEngine;

namespace Kodama.Audio {
    [Serializable]
    public class AudioCue : MonoBehaviour {
        [SerializeField] private bool _playOnStart;
        [SerializeField, Expandable] private AudioCueSo _audioCue;
        [SerializeField, Expandable] private AudioConfigSo _audioConfig;
        [SerializeField] private AudioCueChannelSO _audioCueChannel;

        public AudioCueSo Cue {
            get => _audioCue;
            set => _audioCue = value;
        }

        private void Start() {
            if (_playOnStart) {
                RequestAudio(transform.position);
            }
        }

        private void RequestAudio(Vector3 position) {
            var data = new AudioCueRequestData(_audioCue, _audioConfig, position);
            _audioCueChannel.RequestAudio(data);
        }

        [ContextMenu("Play Audio Cue")]
        public void PlayAudioCue() => RequestAudio(transform.position);

        public void PlayAudioCue(Vector3 position) => RequestAudio(position);
    }
}