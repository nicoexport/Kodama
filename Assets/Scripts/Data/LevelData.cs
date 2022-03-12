using UnityEngine;

public class LevelData
{
    public readonly string ScenePath;
    public readonly string LevelName;
    private Sprite LevelImage;
    public bool Unlocked { get; set; } = false;
    public bool Visited;
    public bool Completed;
    public float RecordTime = Mathf.Infinity;
    

    public LevelData(LevelDataSO levelDataSO)
    {
        this.ScenePath = levelDataSO.ScenePath;
        this.LevelName = levelDataSO.LevelName;
        this.LevelImage = levelDataSO.LevelImage;
    }

    public bool UpdateRecordTime(float time)
    {
        if (!(time < RecordTime)) return false;
        RecordTime = time;
        return true;
    }
}