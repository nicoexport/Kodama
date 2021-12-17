using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "LevelObject")]
public class LevelObject : ScriptableObject
{
    public string levelName;
    public int levelIndex;
    public string scenePath;
    public float RecordTime { get; private set; }

    public void UpdateRecordTime(float time)
    {
        RecordTime = time;
    }
}
