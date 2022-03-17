using UnityEngine;
using TMPro;

public class LevelSelectWorldPanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _worldName;
    [SerializeField] private TextMeshProUGUI _completionText;


    private void OnEnable()
    {
        LevelNavigationManager.OnLevelSelectStart += UpdatePanel;
    }

    private void OnDisable()
    {
        LevelNavigationManager.OnLevelSelectStart -= UpdatePanel;
    }

    private void UpdatePanel(WorldData worldData, LevelData levelData)
    {
        _worldName.text = worldData.WorldName;
    }
}
