using Architecture;
using Level_Selection;
using TMPro;
using UnityEngine;

namespace UI
{
    public class LevelSelectInstructionButton : MonoBehaviour
    {
        [SerializeField] string _worldSelectText;
        [SerializeField] string _levelSelectText;
        string _defaultText;

        TextMeshProUGUI _text;

        void Awake()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _defaultText = _text.text;
        }

        void OnEnable()
        {
            LevelSelectManager.OnSelectUISwitched += ChangeButtonText;
        }

        void OnDisable()
        {
            LevelSelectManager.OnSelectUISwitched -= ChangeButtonText;
        }

        void ChangeButtonText(ISelectUI selectUI)
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