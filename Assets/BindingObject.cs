using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BindingObject : MonoBehaviour
{
    public InputAction inputAction;
    public int bindingIndex;
    [SerializeField] TMP_Text bindingName;
    [SerializeField] TMP_Text keyBinding;

    public void FillInInformation(InputAction inputAction, int bindingIndex)
    {
        this.inputAction = inputAction;
        this.bindingIndex = bindingIndex;

        string bName = inputAction.bindings[bindingIndex].name;
        bindingName.text = bName == null || bName.Length == 0 ? inputAction.name : bName;

        keyBinding.text = InputControlPath.ToHumanReadableString(
                inputAction.bindings[bindingIndex].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    public void RebindInput()
    {
        TitleMenuManager.instance.StartInteractiveRebind(this);
    }

    public void Refresh()
    {
        FillInInformation(inputAction, bindingIndex);
    }
}
