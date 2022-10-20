using Architecture;
using Level_Selection;
using TMPro;
using UnityEngine;

namespace UI
{
    public class LevelSelectInstructionButton : MonoBehaviour
    {
        [SerializeField] private string _worldSelectText;
        [SerializeField] private string _levelSelectText;
        private string _defaultText;

        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _defaultText = _text.text;
        }

        private void OnEnable()
        {
            LevelSelectManager.OnSelectUISwitched += ChangeButtonText;
        }

        private void OnDisable()
        {
            LevelSelectManager.OnSelectUISwitched -= ChangeButtonText;
        }

        private void ChangeButtonText(ISelectUI selectUI)
        {
            switch (selectUI)
            {
                case LevelSelect levelSelect:
                    _text.text = _levelSelectText;
                    break;
                case WorldSelect worldSelect:
                    _text.text = _worldSelectText;
                    break;
                default:
                    _text.text = _defaultText;
                    break;
            }
        }
    }
}