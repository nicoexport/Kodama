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
        [SerializeField] private Image _targetImage;
        [SerializeField] private Sprite _keyboardSprite;
        [SerializeField] private Sprite _dualShockSprite;
        [SerializeField] private Sprite _xboxSprite;
        [SerializeField] private Sprite _nintendoSprite;

        private void Awake()
        {
            OnInputDeviceChange(InputManager.CurrentInputDevice);
        }

        private void OnEnable()
        {
            InputManager.OnInputDeviceChanged += OnInputDeviceChange;
        }

        private void OnDisable()
        {
            InputManager.OnInputDeviceChanged -= OnInputDeviceChange;
        }


        private void OnInputDeviceChange(InputDevice inputDevice)
        {
            var type = InputManager.GetGeneralDeviceTypeByName(inputDevice.name);
            UpdateButtonImage(type);
        }

        private void UpdateButtonImage(GeneralDeviceType? type)
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