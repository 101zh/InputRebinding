using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BindingGameObject : MonoBehaviour
{
    public InputAction inputAction;
    public int bindingIndex;
    [SerializeField] TMP_Text BindingName;
    [SerializeField] TMP_Text keyBinding;

    public void fillInInformation(InputAction inputAction, int bindingIndex)
    {
        this.inputAction = inputAction;
        this.bindingIndex = bindingIndex;
        BindingName.text = inputAction.bindings[bindingIndex].name;
        keyBinding.text = InputControlPath.ToHumanReadableString(
                inputAction.bindings[bindingIndex].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    public void rebindInput()
    {
        SettingsManager.rebindBindingAtIndex(this);
    }

    public void refresh()
    {
        fillInInformation(inputAction, bindingIndex);
    }
}
