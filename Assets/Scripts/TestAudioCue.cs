using Audio;
using UnityEngine;

namespace DefaultNamespace
{
    public class TestAudioCue : MonoBehaviour
    {
        AudioCue _audioCue;

        void Awake()
        {
            _audioCue = GetComponent<AudioCue>();
        }

        [ContextMenu("Test Audio")]
        void TestAudio()
        {
            _audioCue.PlayAudioCue();
        }
    }
}