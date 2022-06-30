using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using World_Level;

public class LevelButton : MonoBehaviour
{
    [SerializeField] LevelObject levelObject;

    Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void OnEnable()
    {
        if (!image) return;
        image.sprite = levelObject.levelImage;
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(levelObject.ScenePath);
    }
}
