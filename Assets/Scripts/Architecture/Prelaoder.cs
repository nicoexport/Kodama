using UnityEngine;
using UnityEngine.SceneManagement;

namespace Architecture
{
    public class Prelaoder : MonoBehaviour
    {
        [SerializeField] int mainMenuIndex = 1;

        void Start()
        {
            SceneManager.LoadScene(mainMenuIndex);
        }
    }
}