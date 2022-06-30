using System;
using UnityEngine;

namespace Level.Logic
{
    public class LevelWin : MonoBehaviour
    {
        public static event Action OnLevelWon;

        public void WinLevel()
        {
            OnLevelWon?.Invoke();
        }
    }
}