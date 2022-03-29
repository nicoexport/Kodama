using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class LevelSelectInstructionButton : MonoBehaviour
    {
        [SerializeField] private string _worldSelectText;
        [SerializeField] private string _levelSelectText;
        
        private TextMeshProUGUI _text;
        private string _defaultText;

        private void OnEnable()
        {
            LevelSelectManager.OnSelectUISwitched += ChangeButtonText;
        }

        private void OnDisable()
        {
            LevelSelectManager.OnSelectUISwitched -= ChangeButtonText;
        }

        private void Awake()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _defaultText = _text.text;
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