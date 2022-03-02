using UnityEngine;

public class LevelFlowHandler : MonoBehaviour
{
    [SerializeField]
    private LoadLevelEventChannelSO _loadLevelEventChannel;
    [SerializeField]
    private GameSessionDataSO _sessionData;

    public void ExitLevel(LevelDataSO levelData)
    {

    }

    // Takes in a LevelDataSO. If its not the Worlds last level it loads the next level of the same world, if it is it loads the next world. 
    public void NextLevelRequest(LevelDataSO levelData)
    {
        Debug.Log("LEVELFLOW: TRYING NEXT LEVEL REQUEST");
        // iterate over all worlds
        for (int i = 0; i < _sessionData.WorldDatas.Count; i++)
        {
            // Iterate over the worlds levels 
            for (int j = 0; j < _sessionData.WorldDatas[i].LevelDatas.Count; j++)
            {
                if (_sessionData.WorldDatas[i].LevelDatas[j] == levelData)
                {
                    // if its not the last level of a world just load the next level of the world.
                    if (j < _sessionData.WorldDatas[i].LevelDatas.Count - 1)
                    {
                        LoadLevel(_sessionData.WorldDatas[i].LevelDatas[j + 1]);
                        return;
                    }

                    // if its the last level of the world and its not the last world load the next world.
                    else if (i < _sessionData.WorldDatas.Count - 1)
                    {
                        LoadWorld(_sessionData.WorldDatas[i + 1]);
                        return;
                    }

                    // if its the last level of the last world load the credits.
                    else
                    {
                        LoadCredits();
                        return;
                    }
                }
            }
        }
    }


    private void LoadLevel(LevelDataSO levelData)
    {
        _sessionData.CurrentLevel = levelData;
        _loadLevelEventChannel.RaiseEventWithLevelData(levelData, true, true);
    }

    private void LoadWorld(WorldDataSO worldData)
    {
        _sessionData.CurrentWorld = worldData;
        //_sessionData.CurrentLevel = worldData.LevelDatas[0];
        _loadLevelEventChannel.RaiseEventWithScenePath(_sessionData.WorldsScenePath, true, true);
        Debug.Log("TO DO: Load Next World");
    }

    private void LoadCredits()
    {
        Debug.Log("TO DO: Load Credits");
    }
}