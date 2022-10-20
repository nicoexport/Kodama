using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Level.Logic
{
    public class LevelReset : MonoBehaviour
    {
        private void Update()
        {
            if (Keyboard.current.lKey.wasPressedThisFrame) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}