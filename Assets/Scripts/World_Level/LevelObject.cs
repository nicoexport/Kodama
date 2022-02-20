using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "SceneObjects/LevelObject")]
public class LevelObject : ScriptableObject
{
    public string levelName;
    public int levelIndex;
    public int worldIndex;
    public Sprite levelImage;
    public string ScenePath;
    public float RecordTime = Mathf.Infinity;
    public bool levelCompleted;

    public void UpdateRecordTime(float time)
    {
        RecordTime = time;
    }

    public void ResetRecord()
    {
        RecordTime = Mathf.Infinity;
    }
}
