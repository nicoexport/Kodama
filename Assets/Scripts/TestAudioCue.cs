using Audio;
using UnityEngine;

namespace DefaultNamespace {
    public class TestAudioCue : MonoBehaviour {
        private AudioCue _audioCue;

        private void Awake() => _audioCue = GetComponent<AudioCue>();

        [ContextMenu("Test Audio")]
        private void TestAudio() => _audioCue.PlayAudioCue();
    }
}