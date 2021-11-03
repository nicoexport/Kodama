using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelReset : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown("l"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
