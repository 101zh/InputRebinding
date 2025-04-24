using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BindingObject : MonoBehaviour
{
    public InputAction inputAction;
    public int bindingIndex;
    [SerializeField] TMP_Text bindingName;
    [SerializeField] TMP_Text keyBinding;

    private void Start()
    {
        SettingsManager.refreshAllBindings += Refresh;
    }

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
