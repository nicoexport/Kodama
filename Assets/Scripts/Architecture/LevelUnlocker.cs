using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelUnlocker : MonoBehaviour
{
    [SerializeField] private LevelFinishedEventChannel _levelFinishedEventChannel;
    [FormerlySerializedAs("_gameSession")] [SerializeField] private SaveDataSo _save;

    private void OnEnable()
    {
        _levelFinishedEventChannel.OnLevelFinished += HandleUnlocks;
    }

    private void OnDisable()
    {
        _levelFinishedEventChannel.OnLevelFinished -= HandleUnlocks;
    }

    private void HandleUnlocks(LevelData obj)
    {
        
        for (int i = 0; i < _save.WorldDatas.Count; i++)
        {
            for (int j = 0; j < _save.WorldDatas[i].LevelDatas.Count; j++)
            {
                if (_save.WorldDatas[i].LevelDatas[j].LevelName == obj.LevelName)
                {
                    // if its not the last level of a world just unlock the next level of the world.
                    if (j < _save.WorldDatas[i].LevelDatas.Count - 1)
                    {
                        _save.WorldDatas[i].LevelDatas[j + 1].Unlocked = true;
                        return;
                    }

                    // if its the last level of the world and its not the last world load the next world.
                    else if (i < _save.WorldDatas.Count - 1)
                    {
                        _save.WorldDatas[i + 1].Unlocked = true;
                        return;
                    }
                }
            }
        }
    }
}
