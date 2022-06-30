using System;
using Architecture;
using UnityEngine;

namespace Level.Logic
{
    public class LevelWin : MonoBehaviour
    {
        public void WinLevel()
        {
            LevelManager.Instance.CompleteLevel();
        }
    }
}