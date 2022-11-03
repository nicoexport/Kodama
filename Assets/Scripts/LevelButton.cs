using Kodama.World_Level;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Kodama
{
    public class LevelButton : MonoBehaviour {
        [SerializeField] private LevelObject levelObject;

        private Image image;

        private void Awake() => image = GetComponent<Image>();

        private void OnEnable() {
            if (!image) {
                return;
            }

            image.sprite = levelObject.levelImage;
        }

        public void LoadLevel() => SceneManager.LoadScene(levelObject.ScenePath);
    }
}