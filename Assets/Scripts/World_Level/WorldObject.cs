using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WorldObject")]
public class WorldObject : ScriptableObject
{
    public string worldName;
    public int worldIndex;
    public Sprite WorldImage;
    public List<LevelObject> LevelObjects = new List<LevelObject>();
    public float RecordTime { get; private set; }

    public void UpdateRecordTime()
    {
        var time = 0f;
        foreach (LevelObject level in LevelObjects)
        {
            time += level.RecordTime;
        }
        RecordTime = time;
    }

    public void UpdateRecordTime2()
    {
        float time = 0f;
        foreach (LevelObject level in LevelObjects)
        {
            if (level.RecordTime == Mathf.Infinity) return;
            time += level.RecordTime;
        }
        RecordTime = time;
    }
}
