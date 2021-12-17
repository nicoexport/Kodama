using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WorldObject")]
public class World : ScriptableObject
{
    public string worldName;
    public int worldIndex;
    public LevelObject[] levels;
    public float RecordTime { get; private set; }

    public void UpdateRecordTime()
    {
        var time = 0f;
        foreach (LevelObject level in levels)
        {
            time += level.RecordTime;
        }
        RecordTime = time;
    }
}
