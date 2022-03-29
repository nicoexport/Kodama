using UnityEngine;
using System;
using System.Collections.Generic;
using Scriptable;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    [SerializeField] private DeviceMapSo _deviceMap;
    public static PlayerInputActions playerInputActions { get; private set; }
    public static event Action<InputActionMap> OnActionMapChange;
    public static event Action<InputDevice> OnInputDeviceChanged;
    public static InputDevice CurrentInputDevice { get; private set; }
    
    private static PlayerInput _playerInput;
    private static readonly Dictionary<string, GeneralDeviceType> _deviceMapDictionary = new Dictionary<string, GeneralDeviceType>();

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
        {
            Debug.LogErrorFormat("InputManager: DeviceMapSo not valid. Try having same list sizes");
        }
        for (var index = 0; index < _deviceMap.DeviceNames.Count; index++)
        {
            var name = _deviceMap.DeviceNames[index];
            var type = _deviceMap.DeviceTypes[index];
            _deviceMapDictionary.Add(name, type);
        }
    }
    
    public static void ToggleActionMap(InputActionMap actionMap)
    {
        if (actionMap.enabled) return;
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
        else
        {
            return null;
        }
    }

}