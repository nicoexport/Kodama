using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField]
    private LevelObject nextLevel;

    public void ExitLevel()
    {
        SceneManager.LoadSceneAsync(nextLevel.ScenePath);
    }
}
