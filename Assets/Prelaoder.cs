using UnityEngine;
using UnityEngine.SceneManagement;

public class Prelaoder : MonoBehaviour
{
    [SerializeField]
    private int mainMenuIndex = 1;

    private void Start()
    {
        SceneManager.LoadScene(mainMenuIndex);
    }
}


