using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField]
    private int nextSceneIndex; // replace with LevelSO that has all information about the next level so the transition screen can use this information 

    public void ExitLevel()
    {
        SceneManager.LoadScene(nextSceneIndex);
    }
}
