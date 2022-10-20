using System;
using System.Collections.Generic;
using Data;
using Scriptable;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Architecture
{
    public class InputManager : MonoBehaviour
    {
        private const float movementDeadZoneMin = 0.5f;
        private const float movementDeadZoneMax = 0.925f;

        private static PlayerInput _playerInput;
        private static readonly Dictionary<string, GeneralDeviceType> _deviceMapDictionary = new();
        [SerializeField] private DeviceMapSo _deviceMap;
        public static PlayerInputActions playerInputActions { get; private set; }
        public static InputDevice CurrentInputDevice { get; private set; }

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            CurrentInputDevice = _playerInput.devices[0];
            playerInputActions = new PlayerInputActions();
            FillDeviceMapDict();
            print("INPUT MANAGER CURRENT INPUT DEVICE: " + CurrentInputDevice);
        }

        private void OnEnable()
        {
            _playerInput.onActionTriggered += OnActionTriggered;
        }

        private void OnDisable()
        {
            _playerInput.onActionTriggered -= OnActionTriggered;
        }

        public static event Action<InputActionMap> OnActionMapChange;
        public static event Action<InputDevice> OnInputDeviceChanged;

        private void OnActionTriggered(InputAction.CallbackContext obj)
        {
            var nextInputDevice = _playerInput.devices[0];
            if (nextInputDevice == CurrentInputDevice) return;
            CurrentInputDevice = nextInputDevice;
            OnInputDeviceChanged?.Invoke(CurrentInputDevice);
        }

        private void FillDeviceMapDict()
        {
            if (_deviceMap.DeviceNames.Count != _deviceMap.DeviceTypes.Count)
                Debug.LogErrorFormat("InputManager: DeviceMapSo not valid. Try having same list sizes");
            for (var index = 0; index < _deviceMap.DeviceNames.Count; index++)
            {
                var name = _deviceMap.DeviceNames[index];
                var type = _deviceMap.DeviceTypes[index];
                _deviceMapDictionary.Add(name, type);
            }
        }

        public static void ToggleActionMap(InputActionMap actionMap)
        {
            if (actionMap.enabled)return;
            playerInputActions.Disable();
            actionMap.Enable();
            OnActionMapChange?.Invoke(actionMap);
        }

        public static void DisableInput()
        {
            playerInputActions.Disable();
        }

        public static GeneralDeviceType? GetGeneralDeviceTypeByName(string deviceName)
        {
            var success = _deviceMapDictionary.TryGetValue(deviceName, out var type);
            if (success)
                return type;
            return null;
        }

        public static float GetHorizontalMovementValue()
        {
            var input = playerInputActions.Player.HorizontalMovement.ReadValue<float>();
            if (Mathf.Abs(input) < movementDeadZoneMin)
                return 0f;
            if (Mathf.Abs(input) > movementDeadZoneMax)
            {
                if (input < 0)
                    return -1f;
                return 1f;
            }
            return input;
        }

        public static float GetVerticalMovementValue()
        {
            var input = playerInputActions.Player.VerticalMovement.ReadValue<float>();
            if (Mathf.Abs(input) < movementDeadZoneMin)
                return 0f;
            if (Mathf.Abs(input) > movementDeadZoneMax)
            {
                if (input < 0)
                    return -1f;
                return 1f;
            }
            return input;
        }
    }
}