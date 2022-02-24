using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInputActions playerInputActions { get; private set; }
    public static event Action<InputActionMap> OnActionMapChange;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }

    public static void ToggleActionMap(InputActionMap actionMap)
    {
        if (actionMap.enabled) return;
        playerInputActions.Disable();
        actionMap.Enable();
        OnActionMapChange?.Invoke(actionMap);
    }

}