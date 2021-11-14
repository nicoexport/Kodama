using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "LevelObject")]
public class Level : ScriptableObject
{
    public string sceneName;
    public int levelIndex;
    public int buildIndex;
    public float RecordTime { get; private set; }

    public void UpdateRecordTime(float time)
    {
        RecordTime = time;
    }
}
