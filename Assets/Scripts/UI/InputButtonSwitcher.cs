using System;
using Architecture;
using Scriptable;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class InputButtonSwitcher : MonoBehaviour
    {
        [SerializeField] Image _targetImage;
        [SerializeField] Sprite _keyboardSprite;
        [SerializeField] Sprite _dualShockSprite;
        [SerializeField] Sprite _xboxSprite;
        [SerializeField] Sprite _nintendoSprite;

        void Awake()
        {
            OnInputDeviceChange(InputManager.CurrentInputDevice);
        }

        void OnEnable()
        {
            InputManager.OnInputDeviceChanged += OnInputDeviceChange;
        }

        void OnDisable()
        {
            InputManager.OnInputDeviceChanged -= OnInputDeviceChange;
        }


        void OnInputDeviceChange(InputDevice inputDevice)
        {
            var type = InputManager.GetGeneralDeviceTypeByName(inputDevice.name);
            UpdateButtonImage(type);
        }

        void UpdateButtonImage(GeneralDeviceType? type)
        {
            switch (type)
            {
                case GeneralDeviceType.Pc:
                    _targetImage.sprite = _keyboardSprite;
                    break;
                case GeneralDeviceType.Mac:
                    _targetImage.sprite = _keyboardSprite;
                    break;
                case GeneralDeviceType.DualShock:
                    _targetImage.sprite = _dualShockSprite;
                    break;
                case GeneralDeviceType.Xbox:
                    _targetImage.sprite = _xboxSprite;
                    break;
                case GeneralDeviceType.Nintendo:
                    _targetImage.sprite = _nintendoSprite;
                    break;
                case null:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}