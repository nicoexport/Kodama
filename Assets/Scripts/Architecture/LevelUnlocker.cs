using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUnlocker : MonoBehaviour
{
    [SerializeField] private LevelFinishedEventChannel _levelFinishedEventChannel;
    [SerializeField] private GameSessionDataSO _gameSession;

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
        
        for (int i = 0; i < _gameSession.WorldDatas.Count; i++)
        {
            for (int j = 0; j < _gameSession.WorldDatas[i].LevelDatas.Count; j++)
            {
                if (_gameSession.WorldDatas[i].LevelDatas[j].LevelName == obj.LevelName)
                {
                    // if its not the last level of a world just unlock the next level of the world.
                    if (j < _gameSession.WorldDatas[i].LevelDatas.Count - 1)
                    {
                        _gameSession.WorldDatas[i].LevelDatas[j + 1].Unlocked = true;
                        return;
                    }

                    // if its the last level of the world and its not the last world load the next world.
                    else if (i < _gameSession.WorldDatas.Count - 1)
                    {
                        _gameSession.WorldDatas[i + 1].Unlocked = true;
                        return;
                    }
                }
            }
        }
    }
}
