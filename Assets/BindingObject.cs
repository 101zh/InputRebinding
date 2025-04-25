using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary> The script attached to a binding prefab </summary>
public class BindingObject : MonoBehaviour
{
    public InputAction inputAction;
    public int bindingIndex;
    [SerializeField] TMP_Text bindingName;
    [SerializeField] TMP_Text keyBinding;

    private void Start()
    {
        SettingsManager.refreshAllBindings += Refresh; // Subscribes to an event that refreshes all binding prefabs
    }

    /// <summary>
    /// Assigns the text fields with the correct characters based off an input action and a binding index
    /// </summary>
    /// <param name="inputAction"> The input action that this particular binding will rebind </param>
    /// <param name="bindingIndex"> The index of the binding within the input action </param>
    public void FillInInformation(InputAction inputAction, int bindingIndex)
    {
        this.inputAction = inputAction;
        this.bindingIndex = bindingIndex;

        string bName = inputAction.bindings[bindingIndex].name;
        bName = bName == null || bName.Length == 0 ? inputAction.name : bName;
        bName = SettingsManager.instance.useHeaderAsLabel && inputAction.bindings[bindingIndex].isPartOfComposite
            ? inputAction.name + ": " + bName : bName;
        bindingName.text = bName;

        keyBinding.text = InputControlPath.ToHumanReadableString(
                inputAction.bindings[bindingIndex].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    public void RebindInput()
    {
        SettingsManager.instance.StartInteractiveRebind(this);
    }

    public void Refresh()
    {
        FillInInformation(inputAction, bindingIndex);
    }
}
