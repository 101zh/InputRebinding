using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class InputManager : MonoBehaviour
{
    static InputManager instance;

    public static SampleInput sampleInput { get; private set; }
    public static InputAction move { get; private set; }
    public static InputAction jump { get; private set; }
    public static InputAction fire { get; private set; }
    public static InputAction ability1 { get; private set; }
    public static InputAction ability2 { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        sampleInput = new SampleInput();

        move = sampleInput.Player.Movement;
        jump = sampleInput.Player.Jump;
        fire = sampleInput.Player.Fire;
        ability1 = sampleInput.Player.Ability1;
        ability2 = sampleInput.Player.Ability2;
    }

    private void OnEnable()
    {
        move.Enable();
        jump.Enable();
        fire.Enable();
        ability1.Enable();
        ability2.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        fire.Disable();
        ability1.Disable();
        ability2.Disable();
    }

    public static void RebindBindingAtIndex(InputAction inputAction, int index, Action onCancel, Action onComplete)
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
