using Kodama.Architecture;
using UnityEngine;

namespace Kodama.Level.Logic {
    public class LevelWin : MonoBehaviour {
        public void WinLevel() => LevelManager.Instance.CompleteLevel();
    }
}