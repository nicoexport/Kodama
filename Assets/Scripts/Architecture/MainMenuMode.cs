using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kodama.Architecture {
    public class MainMenuMode : IGameMode {
        private readonly string _scenePath;

        public MainMenuMode(string scenePath) => _scenePath = scenePath;

        public GameModeState _state { get; private set; } = GameModeState.Ended;
        public string _activeScene { get; private set; }

        public IEnumerator OnStart() {
            if (_state != GameModeState.Ended) {
                yield break;
            }

            _state = GameModeState.Starting;

            _activeScene = _scenePath;

            yield return SceneManager.LoadSceneAsync(_activeScene, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByPath(_activeScene));

            Debug.Log("MAIN MENU MODE STARTED");
            _state = GameModeState.Started;
        }

        public IEnumerator OnEnd() {
            _state = GameModeState.Ending;
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            Debug.Log("MAIN MENU MODE ENDED");
            _state = GameModeState.Ended;
        }

        public void OnEditorStart() {
            _activeScene = SceneManager.GetActiveScene().path;
            _state = GameModeState.Started;
        }
    }
}