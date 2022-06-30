using System;
using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(fileName = "new AudioCueChannel", menuName = "Channels/AudioCueChannel", order = 0)]
    public class AudioCueChannelSo : ScriptableObject
    {
        public event Action<AudioCueRequestData> OnAudioCueRequested;

        public void RequestAudio(AudioCueRequestData audioCueRequestData)
        {
            OnAudioCueRequested?.Invoke(audioCueRequestData);
        }
    }

    public readonly struct AudioCueRequestData
    {
        public AudioCueRequestData(AudioCueSo audioCue, AudioConfigSo audioConfig, Vector3 position = default)
        {
            AudioCue = audioCue;
            AudioConfig = audioConfig;
            Position = position;
        }

        public readonly AudioCueSo AudioCue;
        public readonly AudioConfigSo AudioConfig;
        public readonly Vector3 Position;
    }
}