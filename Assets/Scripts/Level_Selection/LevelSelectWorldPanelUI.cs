using Data;
using TMPro;
using UnityEngine;

namespace Level_Selection
{
    public class LevelSelectWorldPanelUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _worldName;
        [SerializeField] TextMeshProUGUI _completionText;


        void OnEnable()
        {
            LevelSelect.OnLevelSelectStarted += UpdatePanel;
        }

        void OnDisable()
        {
            LevelSelect.OnLevelSelectStarted -= UpdatePanel;
        }

        void UpdatePanel(WorldData worldData)
        {
            _worldName.text = worldData.WorldName;
        }
    }
}