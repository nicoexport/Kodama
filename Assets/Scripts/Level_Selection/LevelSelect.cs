using System;
using System.Collections;
using System.Collections.Generic;
using Architecture;
using Data;
using Scriptable;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Level_Selection
{
    public class LevelSelect : MonoBehaviour, ISelectUI
    {
        [FormerlySerializedAs("sockets")] [SerializeField]
        List<LevelSelectSocket> _sockets = new();

        [SerializeField] GameObject _uIPlayerObject;
        [SerializeField] float _playerMoveTimeInSeconds = 0.5f;
        [SerializeField] GameObject _ui;

        EventSystem _eventSystem;
        IUICharacter _uiPlayer;

        void Awake()
        {
            _eventSystem = FindObjectOfType<EventSystem>();
            _uiPlayer = _uIPlayerObject.GetComponent<IUICharacter>();
            _ui.SetActive(false);
        }

        void OnEnable()
        {
            LevelSelectSocket.OnButtonSelectedAction += MoveUIPlayer;
        }


        void OnDisable()
        {
            LevelSelectSocket.OnButtonSelectedAction -= MoveUIPlayer;
            InputManager.playerInputActions.LevelSelectUI.Exit.started -= ReturnToWorldSelect;
        }

        public SelectUIState _state { get; private set; }

        public IEnumerator OnStart(SessionData sessionData)
        {
            _state = SelectUIState.Starting;
            _ui.SetActive(true);
            yield return SetupSockets(sessionData.CurrentWorld, sessionData.CurrentLevel);
            OnLevelSelectStarted?.Invoke(sessionData.CurrentWorld);
            InputManager.playerInputActions.LevelSelectUI.Exit.started += ReturnToWorldSelect;
            _state = SelectUIState.Started;
        }

        public IEnumerator OnEnd()
        {
            _state = SelectUIState.Ending;
            InputManager.playerInputActions.LevelSelectUI.Exit.started -= ReturnToWorldSelect;
            _ui.SetActive(false);
            _state = SelectUIState.Ending;
            yield break;
        }

        public static event Action<WorldData> OnLevelSelectStarted;
        public event Action OnReturnToWorldSelect;

        IEnumerator SetupSockets(WorldData worldData, LevelData currentLevelData)
        {
            foreach (var socket in _sockets) socket.gameObject.SetActive(false);

            var socketAmount = worldData.LevelDatas.Count;
            if (socketAmount > _sockets.Count)
                yield break;

            for (var i = 0; i < socketAmount; i++)
            {
                var socket = _sockets[i];
                socket.gameObject.SetActive(true);
                socket.SetupSocket(worldData, worldData.LevelDatas[i], i >= socketAmount - 1, i + 1);
                socket.SetButtonInteractable(worldData.LevelDatas[i].Unlocked);
                if (worldData.LevelDatas[i] == currentLevelData)
                {
                    _uIPlayerObject.transform.position = socket.Button.transform.position;
                    SetActiveButton(socket.Button);
                }
            }
        }

        void SetActiveButton(Button button)
        {
            _eventSystem.SetSelectedGameObject(button.gameObject);
        }

        void ReturnToWorldSelect(InputAction.CallbackContext obj)
        {
            OnReturnToWorldSelect?.Invoke();
        }


        void MoveUIPlayer(LevelData levelData, Transform transform1)
        {
            if (_state != SelectUIState.Started) return;
            LeanTween.cancel(_uIPlayerObject);
            _eventSystem.enabled = false;
            _uiPlayer.StartMoving(transform1);
            LeanTween.move(_uIPlayerObject, transform1.position, _playerMoveTimeInSeconds)
                .setOnComplete(() =>
                {
                    _eventSystem.enabled = true;
                    _uiPlayer.StopMoving();
                });
        }
    }
}