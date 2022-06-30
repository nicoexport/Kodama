using Data;
using Scriptable;
using UnityEngine;
using UnityEngine.Serialization;

namespace Architecture
{
    public class LevelUnlocker : MonoBehaviour
    {
        [FormerlySerializedAs("_gameSession")] [SerializeField]
        SessionData _session;

        void OnEnable()
        {
            LevelManager.OnLevelComplete += HandleUnlocks;
        }

        void OnDisable()
        {
            LevelManager.OnLevelComplete -= HandleUnlocks;
        }

        void HandleUnlocks(LevelData obj)
        {
            for (var i = 0; i < _session.WorldDatas.Count; i++)
            for (var j = 0; j < _session.WorldDatas[i].LevelDatas.Count; j++)
                if (_session.WorldDatas[i].LevelDatas[j].LevelName == obj.LevelName)
                {
                    // if its not the last level of a world just unlock the next level of the world.
                    if (j < _session.WorldDatas[i].LevelDatas.Count - 1)
                    {
                        _session.WorldDatas[i].LevelDatas[j + 1].Unlocked = true;
                        return;
                    }

                    // if its the last level of the world and its not the last world load the next world.

                    if (i < _session.WorldDatas.Count - 1)
                    {
                        _session.WorldDatas[i + 1].Unlocked = true;
                        return;
                    }
                }
        }
    }
}