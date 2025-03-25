using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class InputRebinding
{
    public static void rebindBindingAtIndex(InputAction inputAction, int index, Action onCancel, Action onComplete)
    {
        inputAction.Disable();

        RebindingOperation rebinding = inputAction.PerformInteractiveRebinding(index)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnCancel(operation => onCancel())
            .OnComplete(operation => onComplete());

        rebinding.Start();

        inputAction.Enable();
    }
}
