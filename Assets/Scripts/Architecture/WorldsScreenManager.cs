using Data;
using Scriptable;
using TMPro;
using UnityEngine;
using Utility;

namespace Architecture
{
    public class WorldsScreenManager : Singleton<WorldsScreenManager>
    {
        [SerializeField] SessionData _sessionData;

        [SerializeField] LoadLevelEventChannelSO _loadLevelEventChannel;


        [Header("Temporary")] [SerializeField] TextMeshProUGUI worldDisplay;

        [SerializeField] TextMeshProUGUI recordDisplay;
        LevelData _currentLevel;

        WorldData _currentWorld;

        protected override void Awake()
        {
            base.Awake();
            _currentWorld = _sessionData.CurrentWorld;
            _currentLevel = _sessionData.CurrentLevel;
            UpdateWorldDisplay();
        }

        void Start()
        {
            // StartTestLevel();
        }


        [ContextMenu("StartLevel")]
        public void StartLevel()
        {
            var level = _currentLevel;
            _loadLevelEventChannel.RaiseEventWithScenePath(level.ScenePath, true, true);
        }


        void UpdateWorldDisplay()
        {
            worldDisplay.text = _currentWorld.WorldName + ": " + _currentLevel.LevelName;
            recordDisplay.text = _currentLevel.RecordTime == Mathf.Infinity
                ? "NO RECORD"
                : _currentLevel.RecordTime.ToString();
        }

        [ContextMenu("ReturnToMainMenu")]
        void ReturnToMainMenu()
        {
            GameModeManager.Instance.HandleModeStartRequested(GameModeManager.Instance.mainMenuMode);
        }
    }
}