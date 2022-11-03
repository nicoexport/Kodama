using Kodama.Data;
using Kodama.Scriptable;
using Kodama.Scriptable.Channels;
using UnityEngine;

namespace Kodama.Architecture {
    public class LevelUnlocker : MonoBehaviour {
        [SerializeField] private SessionData _session;

        [Header("Channels")] [SerializeField] private LevelDataEventChannelSO _onLevelCompleteChannel;

        private void OnEnable() => _onLevelCompleteChannel.OnEventRaised += HandleUnlocks;

        private void OnDisable() => _onLevelCompleteChannel.OnEventRaised -= HandleUnlocks;

        private void HandleUnlocks(LevelData obj) {
            if (obj == null) {
                return;
            }

            for (int i = 0; i < _session.WorldDatas.Count; i++)
            for (int j = 0; j < _session.WorldDatas[i].LevelDatas.Count; j++) {
                if (_session.WorldDatas[i].LevelDatas[j].LevelName == obj.LevelName) {
                    // if its not the last level of a world just unlock the next level of the world.
                    if (j < _session.WorldDatas[i].LevelDatas.Count - 1) {
                        _session.WorldDatas[i].LevelDatas[j + 1].Unlocked = true;
                        return;
                    }

                    // if its the last level of the world and its not the last world load the next world.
                    if (i < _session.WorldDatas.Count - 1) {
                        _session.WorldDatas[i + 1].Unlocked = true;
                        return;
                    }
                }
            }
        }
    }
}