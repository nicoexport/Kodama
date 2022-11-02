using Data;
using Scriptable;
using Scriptable.Channels;
using TMPro;
using UnityEngine;
using Utility;

namespace Architecture {
    public class WorldsScreenManager : Singleton<WorldsScreenManager> {
        [SerializeField] private SessionData _sessionData;

        [SerializeField] private LoadLevelEventChannelSO _loadLevelEventChannel;


        [Header("Temporary")] [SerializeField] private TextMeshProUGUI worldDisplay;

        [SerializeField] private TextMeshProUGUI recordDisplay;
        private LevelData _currentLevel;

        private WorldData _currentWorld;

        protected override void Awake() {
            base.Awake();
            _currentWorld = _sessionData.CurrentWorld;
            _currentLevel = _sessionData.CurrentLevel;
            UpdateWorldDisplay();
        }

        private void Start() {
            // StartTestLevel();
        }


        [ContextMenu("StartLevel")]
        public void StartLevel() {
            var level = _currentLevel;
            _loadLevelEventChannel.RaiseEventWithScenePath(level.ScenePath, true, true);
        }


        private void UpdateWorldDisplay() {
            worldDisplay.text = _currentWorld.WorldName + ": " + _currentLevel.LevelName;
            recordDisplay.text = _currentLevel.RecordTime == Mathf.Infinity
                ? "NO RECORD"
                : _currentLevel.RecordTime.ToString();
        }

        [ContextMenu("ReturnToMainMenu")]
        private void ReturnToMainMenu() =>
            GameModeManager.Instance.HandleModeStartRequested(GameModeManager.Instance.mainMenuMode);
    }
}