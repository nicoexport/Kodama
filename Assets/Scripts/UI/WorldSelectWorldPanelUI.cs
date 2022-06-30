using Data;
using TMPro;
using UnityEngine;

namespace UI
{
    public class WorldSelectWorldPanelUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _worldName;
        [SerializeField] TextMeshProUGUI _completionText;

        void OnEnable()
        {
            WorldSelectSocket.OnButtonSelectedAction += UpdatePanel;
        }

        void OnDisable()
        {
            WorldSelectSocket.OnButtonSelectedAction -= UpdatePanel;
        }

        void UpdatePanel(WorldData worldData, Transform transform1)
        {
            _worldName.text = worldData.WorldName;
        }
    }
}