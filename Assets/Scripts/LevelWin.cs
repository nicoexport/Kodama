using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LevelWin : MonoBehaviour
{
    public event Action OnLevelWon;

    public void WinLevel()
    {
        OnLevelWon?.Invoke();
    }
}
