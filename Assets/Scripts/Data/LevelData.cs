using UnityEngine;

public class LevelData
{
    public string ScenePath;
    public string LevelName;
    public Sprite LevelImage;
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
        if (time < RecordTime)
        {
            RecordTime = time;
            return true;
        }
        return false;
    }
}