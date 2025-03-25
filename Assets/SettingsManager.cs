using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsManager : MonoBehaviour
{
    SampleInput sampleInput;
    [SerializeField] Vector2 moveDir;
    InputAction move;
    InputAction jump;
    [SerializeField] InputAction[] allPlayerControls;
    [SerializeField] List<InputBinding> allBindings;
    [SerializeField] GameObject bindingObj;
    [SerializeField] Transform bindingContainer;
    [SerializeField] GameObject cover;

    static GameObject coverGameObj;

    // Start is called before the first frame update
    void Start()
    {
        sampleInput = new SampleInput();
        move = sampleInput.Player.Move;
        jump = sampleInput.Player.Jump;
        jump.Enable();
        move.Enable();

        coverGameObj = cover;

        allPlayerControls = sampleInput.Player.Get().ToArray();
        for (int i = 0; i < allPlayerControls.Length; i++)
        {
            if (allPlayerControls[i].bindings[0].isComposite)
            {
                for (int j = 1; j < allPlayerControls[i].bindings.Count; j++)
                {
                    GameObject g = Instantiate(bindingObj, bindingContainer);
                    g.GetComponent<BindingGameObject>().fillInInformation(allPlayerControls[i], j);
                }
            }
            else
            {
                GameObject g = Instantiate(bindingObj, bindingContainer);
                g.GetComponent<BindingGameObject>().fillInInformation(allPlayerControls[i], 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (jump.WasPressedThisFrame())
        {
            Debug.Log("jump");
        }
        moveDir = move.ReadValue<Vector2>();
    }

    public static void rebindBindingAtIndex(BindingGameObject bindingGameObject)
    {
        coverGameObj.SetActive(true);
        InputRebinding.rebindBindingAtIndex(bindingGameObject.inputAction, bindingGameObject.bindingIndex, onRebindCancel, () => onRebindComplete(bindingGameObject));
    }

    private static void onRebindComplete(BindingGameObject bindingGameObject)
    {
        coverGameObj.SetActive(false);
        bindingGameObject.refresh();
        Debug.Log("completed");
    }

    private static void onRebindCancel()
    {
        Debug.Log("canceled");
    }

}
