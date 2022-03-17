using UnityEngine;
using TMPro;

public class LevelSelectWorldPanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _worldName;
    [SerializeField] private TextMeshProUGUI _completionText;


    private void OnEnable()
    {
        LevelSelect.OnLevelSelectStarted += UpdatePanel;
    }

    private void OnDisable()
    {
        LevelSelect.OnLevelSelectStarted -= UpdatePanel;
    }

    private void UpdatePanel(WorldData worldData)
    {
        _worldName.text = worldData.WorldName;
    }
}
