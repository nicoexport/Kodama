using System.Collections;
using Kodama.Architecture;
using Kodama.Data;
using Kodama.Scriptable;
using Kodama.Scriptable.Channels;
using Kodama.Utility;
using Plugins.LeanTween.Framework;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Kodama.UI {
    public class WorldSelect : MonoBehaviour, ISelectUI {
        [SerializeField] private GameObject _ui;
        [SerializeField] private GameObject _socketsParent;
        [SerializeField] private GameObject _socketPrefab;
        [SerializeField] private GameObject _uIPlayerObject;
        [SerializeField] private float _playerMoveTimeInSeconds = 0.5f;
        [SerializeField] private VoidEventChannelSO _returnToMainMenuChannel;

        private EventSystem _eventSystem;
        private IUICharacter _uiPlayer;

        private void Awake() {
            _eventSystem = FindObjectOfType<EventSystem>();
            _uiPlayer = _uIPlayerObject.GetComponent<IUICharacter>();
            _ui.SetActive(false);
        }

        public IEnumerator Reset(SessionData sessionData) {
            yield return ClearSockets();
            yield return OnStart(sessionData);
        }

        private void OnEnable() => WorldSelectSocket.OnButtonSelectedAction += MoveUIPlayer;

        private void OnDisable() {
            WorldSelectSocket.OnButtonSelectedAction -= MoveUIPlayer;
            InputManager.playerInputActions.LevelSelectUI.Exit.started -= HandleExit;
        }

        public SelectUIState _state { get; private set; }

        public IEnumerator OnStart(SessionData sessionData) {
            _state = SelectUIState.Starting;
            _ui.SetActive(true);
            yield return SetupUI(sessionData);
            _uIPlayerObject.transform.position = _eventSystem.currentSelectedGameObject.transform.position;
            MoveUIPlayer(sessionData.CurrentWorld, _eventSystem.currentSelectedGameObject.transform);
            InputManager.playerInputActions.LevelSelectUI.Exit.started += HandleExit;
            _state = SelectUIState.Started;
        }

        public IEnumerator OnEnd() {
            _state = SelectUIState.Ending;
            InputManager.playerInputActions.LevelSelectUI.Exit.started -= HandleExit;
            yield return ClearSockets();
            _ui.SetActive(false);
            _state = SelectUIState.Ended;
        }


        private IEnumerator SetupUI(SessionData sessionData) {
            yield return SetupSockets(sessionData);
        }

        private IEnumerator SetupSockets(SessionData sessionData) {
            for (int index = 0; index < sessionData.WorldDatas.Count; index++) {
                var worldData = sessionData.WorldDatas[index];
                var socketGameObject = Instantiate(_socketPrefab, _socketsParent.transform.position,
                    quaternion.identity,
                    _socketsParent.transform);
                var socket = socketGameObject.GetComponent<WorldSelectSocket>();
                socket.SetupSocket(worldData, index >= sessionData.WorldDatas.Count - 1, index);

                if (worldData == sessionData.CurrentWorld) {
                    _eventSystem.SetSelectedGameObject(socket.Button.gameObject);
                }
            }

            yield break;
        }

        private void MoveUIPlayer(WorldData worldData, Transform transform1) {
            if (_state != SelectUIState.Started) {
                return;
            }

            LeanTween.cancel(_uIPlayerObject);
            _eventSystem.enabled = false;
            _uiPlayer.StartMoving(transform1);
            LeanTween.move(_uIPlayerObject, transform1.position, _playerMoveTimeInSeconds)
                .setOnComplete(() => {
                    _eventSystem.enabled = true;
                    _uiPlayer.StopMoving();
                });
        }


        private IEnumerator ClearSockets() {
            _socketsParent.transform.DeleteChildren();
            yield break;
        }

        private void HandleExit(InputAction.CallbackContext obj) {
            _eventSystem.enabled = false;
            GameModeManager.Instance.HandleModeStartRequested(GameModeManager.Instance.mainMenuMode);
        }
    }
}