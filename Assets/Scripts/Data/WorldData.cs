using UnityEngine;
using System.Collections.Generic;

public class WorldData
{
    public string WorldName;
    public bool Visited;
    public bool Completed;

    public List<LevelData> LevelDatas = new List<LevelData>();

    public WorldData(WorldDataSO worldDataSO)
    {
        this.WorldName = worldDataSO.WorldName;
    }
}