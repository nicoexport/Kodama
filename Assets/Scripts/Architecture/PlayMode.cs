using System.Collections;
using Kodama.SaveLoad;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kodama.Architecture {
    public class PlayMode : IGameMode {
        private readonly string _scenePath;

        public PlayMode(string scenePath) => _scenePath = scenePath;

        public GameModeState _state { get; private set; } = GameModeState.Ended;
        public string _activeScene { get; private set; }

        public IEnumerator OnStart() {
            if (_state != GameModeState.Ended) {
                yield break;
            }

            _state = GameModeState.Starting;

            SaveManager.Instance.OnLoad();

            _activeScene = _scenePath;

            yield return SceneManager.LoadSceneAsync(_activeScene, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByPath(_activeScene));
            Debug.Log("PLAY MODE STARTED");
            _state = GameModeState.Started;
        }

        public IEnumerator OnEnd() {
            _state = GameModeState.Ending;
            SaveManager.Instance.OnSave();
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            Debug.Log("PLAY MODE ENDED");
            _state = GameModeState.Ended;
        }

        public void OnEditorStart() {
#if UNITY_EDITOR
            SaveManager.Instance.OnLoad();
            _activeScene = SceneManager.GetActiveScene().path;
            _state = GameModeState.Started;
#endif
        }
    }
}