using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputActionRebindingExtensions;

public class InputManager : MonoBehaviour
{
    static InputManager instance;

    public static SampleInput sampleInput { get; private set; }
    public static InputActionMap scope { get; private set; }
    public static InputAction move { get; private set; }
    public static InputAction jump { get; private set; }
    public static InputAction fire { get; private set; }
    public static InputAction ability1 { get; private set; }
    public static InputAction ability2 { get; private set; }

    [SerializeField] private bool allowDuplicateBindings = false;

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

        scope = sampleInput.Player; // Assigns which input action map should be allowed to be rebinded

        // Loads in saved rebindings if there are any
        string rebindsJSON = PlayerPrefs.GetString("rebinds");
        if (!(rebindsJSON == null || rebindsJSON.Length == 0))
        {
            scope.LoadBindingOverridesFromJson(rebindsJSON);
        }

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

    /// <summary> Resets all bindings to the default </summary>
    public static void resetKeyBindings()
    {
        scope.RemoveAllBindingOverrides();
        saveBindingOverridesAsJson();
    }

    /// <summary> Rebinds a particular binding. </summary>
    /// <param name="bindingObject"> The binding script on a binding prefab </param>
    /// <param name="onCancel"> The method that should be run if the rebinding operation is canceled </param>
    /// <param name="onComplete"> The method that should be run if the rebinding operation has completed </param>
    public static void RebindBindingAtIndex(BindingObject bindingObject, Action onCancel, Action<BindingObject, bool> onComplete)
    {
        InputAction inputAction = bindingObject.inputAction;
        int bindingIndex = bindingObject.bindingIndex;

        inputAction.Disable();

        InputBinding initialBinding = inputAction.bindings[bindingIndex];

        RebindingOperation rebinding = inputAction.PerformInteractiveRebinding(bindingIndex)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnCancel(operation => onCancel())
            .OnComplete(operation =>
            {
                bool isDuplicateBinding = !instance.allowDuplicateBindings && checkForDuplicateBinding(inputAction, bindingIndex, inputAction.bindings[bindingIndex].isPartOfComposite);
                if (isDuplicateBinding)
                {
                    inputAction.ApplyBindingOverride(initialBinding);
                }
                else
                {
                    saveBindingOverridesAsJson();
                }
                onComplete(bindingObject, isDuplicateBinding);
            });

        rebinding.Start();

        inputAction.Enable();
    }

    private static bool checkForDuplicateBinding(InputAction action, int newBindingIndex, bool checksCompositeParts = true)
    {
        InputBinding newBinding = action.bindings[newBindingIndex];

        foreach (InputBinding b in action.actionMap.bindings)
        {
            if (!(b.action.Equals(newBinding.action)) && b.effectivePath.Equals(newBinding.effectivePath))
            {
                Debug.Log("Duplicate Binding Found!");
                return true;
            }
        }


        if (checksCompositeParts)
        {
            for (int i = 1; i < action.bindings.Count; i++)
            {
                if (i != newBindingIndex && action.bindings[i].effectivePath.Equals(newBinding.overridePath))
                {
                    Debug.Log("Duplicate Binding Found!");
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary> Saves the scope's rebindings as a player preference </summary>
    private static void saveBindingOverridesAsJson()
    {
        PlayerPrefs.SetString("rebinds", scope.SaveBindingOverridesAsJson());
    }
}
