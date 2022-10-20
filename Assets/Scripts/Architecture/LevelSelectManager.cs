using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Data;
using Level_Selection;
using Scriptable;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Utility;

namespace Architecture
{
    [SuppressMessage("ReSharper", "CheckNamespace")]
    public class LevelSelectManager : MonoBehaviour
    {
        [FormerlySerializedAs("_saveData")] [SerializeField]
        private SessionData _sessionData;

        [SerializeField] private WorldDataSO _defaultWorld;

        [FormerlySerializedAs("loadEvenChannel")] [SerializeField]
        private LoadLevelEventChannelSO _loadEvenChannel;

        [SerializeField] private TransitionEventChannelSO _transitionEventChannel;
        [SerializeField] private float _transitionDurationInSeconds;

        [SerializeField] private LevelSelect _levelSelect;
        [SerializeField] private WorldSelect _worldSelect;
        private ISelectUI _currentUI;
        private EventSystem _eventSystem;

        private bool _isSwitching;
        private LevelData _selectedLevel;

        private WorldData _selectedWorld;
        private WaitForSeconds _waitForTransition;

        private void Awake()
        {
            _waitForTransition = new WaitForSeconds(_transitionDurationInSeconds);
            _eventSystem = FindObjectOfType<EventSystem>();

            _selectedWorld = _sessionData.CurrentWorld;
            _selectedLevel = _selectedWorld.LevelDatas.Contains(_sessionData.CurrentLevel)
                ? _sessionData.CurrentLevel
                : _selectedWorld.LevelDatas[0];
            _selectedWorld ??= Utilities.GetWorldDataFromWorldDataSO(_defaultWorld, _sessionData);
            if (_sessionData.FreshSave)
            {
                _sessionData.FreshSave = false;
                SwitchUI(_worldSelect);
            }
            else
            {
                SwitchUI(_levelSelect);
            }
        }

        private void OnEnable()
        {
            LevelSelectSocket.OnButtonClickedAction += LoadLevel;
            WorldSelectSocket.OnButtonClickedAction += OpenWorld;
            _levelSelect.OnReturnToWorldSelect += HandleReturnToWorldSelectRequest;
        }

        private void OnDisable()
        {
            LevelSelectSocket.OnButtonClickedAction -= LoadLevel;
            WorldSelectSocket.OnButtonClickedAction -= OpenWorld;
            _levelSelect.OnReturnToWorldSelect -= HandleReturnToWorldSelectRequest;
        }

        public static event Action<ISelectUI> OnSelectUISwitched;

        private void SwitchUI(ISelectUI selectUI)
        {
            StartCoroutine(SwitchUIEnumerator(selectUI));
        }

        private IEnumerator SwitchUIEnumerator(ISelectUI selectUI)
        {
            yield return new WaitUntil(() => !_isSwitching);
            if (_currentUI == selectUI) yield break;

            _isSwitching = true;
            DisablePlayerInput();
            _transitionEventChannel.RaiseEvent(TransitionType.FadeOut, 0f);


            if (_currentUI != null)
            {
                _transitionEventChannel.RaiseEvent(TransitionType.FadeOut, _transitionDurationInSeconds);
                yield return _waitForTransition;
                yield return _currentUI.OnEnd();
            }

            _currentUI = selectUI;

            yield return _currentUI.OnStart(_sessionData);
            OnSelectUISwitched?.Invoke(_currentUI);
            _transitionEventChannel.RaiseEvent(TransitionType.FadeIn, _transitionDurationInSeconds);
            yield return _waitForTransition;
            EnablePlayerInput();

            _isSwitching = false;
        }

        private void LoadLevel(LevelData obj)
        {
            _eventSystem.enabled = false;
            InputManager.ToggleActionMap(InputManager.playerInputActions.Player);
            _loadEvenChannel.RaiseEventWithScenePath(obj.ScenePath, true, true);
        }

        private void OpenWorld(WorldData worldData)
        {
            if (!ReferenceEquals(_currentUI, _worldSelect)) return;
            _sessionData.CurrentWorld = worldData;
            _sessionData.CurrentLevel = _sessionData.CurrentWorld.LevelDatas[0];

            SwitchUI(_levelSelect);
        }

        private void HandleReturnToWorldSelectRequest()
        {
            SwitchUI(_worldSelect);
        }

        private void EnablePlayerInput()
        {
            _eventSystem.enabled = true;
            InputManager.ToggleActionMap(InputManager.playerInputActions.LevelSelectUI);
            print("Player Input enabled");
        }

        private void DisablePlayerInput()
        {
            _eventSystem.enabled = false;
            InputManager.playerInputActions.Disable();
            print("Player Input disabled");
        }

        #region ContextMenu

        [ContextMenu("UnlockLevels")]
        private void TestUnlockLevelsOfCurrentWorld()
        {
            if ((LevelSelect) _currentUI != _levelSelect) return;
            foreach (var level in _sessionData.CurrentWorld.LevelDatas) level.Unlocked = true;

            StartCoroutine(_levelSelect.OnStart(_sessionData));
        }

        [ContextMenu("UnlockWorlds")]
        private void TestUnlockWorlds()
        {
            if ((WorldSelect) _currentUI != _worldSelect) return;

            foreach (var worldData in _sessionData.WorldDatas) worldData.Unlocked = true;

            StartCoroutine(_worldSelect.Reset(_sessionData));
        }


        [ContextMenu("SwitchToWorldSelect")]
        private void TestSwitchToWorldSelect()
        {
            SwitchUI(_worldSelect);
        }

        [ContextMenu("SwitchToLevelSelect")]
        private void TestSwitchToLevelSelect()
        {
            SwitchUI(_levelSelect);
        }

        #endregion
    }

    public enum LevelSelectionState
    {
        Level,
        World
    }
}