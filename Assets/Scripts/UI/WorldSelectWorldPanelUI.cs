using Data;
using TMPro;
using UnityEngine;

namespace UI {
    public class WorldSelectWorldPanelUI : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI _worldName;
        [SerializeField] private TextMeshProUGUI _completionText;

        private void OnEnable() => WorldSelectSocket.OnButtonSelectedAction += UpdatePanel;

        private void OnDisable() => WorldSelectSocket.OnButtonSelectedAction -= UpdatePanel;

        private void UpdatePanel(WorldData worldData, Transform transform1) => _worldName.text = worldData.WorldName;
    }
}