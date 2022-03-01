using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Game Data/Level Data")]
public class LevelDataSO : ScriptableObject
{
    public string ScenePath;
    public string LevelName;
    public Sprite LevelImage;
    public bool Visited;
    public bool Completed;

    public float RecordTime = Mathf.Infinity;

    public void UpdateRecordTime(float time)
    {
        RecordTime = time;
    }
}