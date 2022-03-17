using UnityEngine;
using System.Collections.Generic;
using Scriptable;

public class WorldData
{
    public string WorldName;
    public bool Visited;
    public bool Completed;
    private bool _unlocked;
    public WorldStyleSo WorldStyleSo;

    public bool Unlocked
    {
        get => _unlocked;
        set
        {
            _unlocked = value;
            if (value == true)
                LevelDatas[0].Unlocked = true;
        }
    }
    
    public List<LevelData> LevelDatas = new List<LevelData>();

    public WorldData(WorldDataSO worldDataSo)
    {
        this.WorldName = worldDataSo.WorldName;
        this.WorldStyleSo = worldDataSo.WorldStyleSo;
    }
    
    
}