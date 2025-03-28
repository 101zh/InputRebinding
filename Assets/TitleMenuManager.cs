using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleMenuManager : MonoBehaviour
{
    public static TitleMenuManager instance { get; private set; }

    /// <summary> A prefab that contains the binding name and a button to reassign that binding. </summary>
    [SerializeField] GameObject bindingObj;
    /// <summary> A prefab that acts as a header for each section of inputs. </summary>
    [SerializeField] GameObject controlHeader;
    /// <summary> The transform that stores each separate binding. </summary>
    [SerializeField] Transform bindingContainer;
    /// <summary> A gameobject that dims the screen when active. </summary>
    [SerializeField] GameObject screenDimmer;

    [SerializeField] List<TMP_Text> titleScreenText;

    static GameObject screenDimmerObj;

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
    }

    void Start()
    {
        screenDimmerObj = screenDimmer;

        AutoGenerateBindings(InputManager.sampleInput, bindingContainer, controlHeader, bindingObj);
    }

    void Update()
    {
        titleScreenText[0].text = "Move Direction: " + InputManager.move.ReadValue<Vector2>();
        titleScreenText[1].text = "Fire is pressed: " + InputManager.fire.IsPressed();
        titleScreenText[2].text = "Jump is pressed: " + InputManager.jump.IsPressed();
        titleScreenText[3].text = "Ability 1 is pressed: " + InputManager.ability1.IsPressed();
        titleScreenText[4].text = "Ability 2 is pressed: " + InputManager.ability2.IsPressed();
    }

    public void StartInteractiveRebind(BindingGameObject bindingGameObject)
    {
        screenDimmerObj.SetActive(true);
        InputManager.RebindBindingAtIndex(bindingGameObject.inputAction,
            bindingGameObject.bindingIndex,
            OnRebindCancel,
            () => OnRebindComplete(bindingGameObject));
    }

    private void OnRebindComplete(BindingGameObject bindingGameObject)
    {
        screenDimmerObj.SetActive(false);
        bindingGameObject.Refresh();
        Debug.Log("completed");
    }

    private void OnRebindCancel()
    {
        screenDimmerObj.SetActive(false);
        Debug.Log("canceled");
    }

    /// <summary>
    /// A method that auto fills a container with binding prefabs.
    /// </summary>
    /// <param name="sampleInput"> An instance of the generated C# class from the input action asset </param>
    /// <param name="containerForBindings"> The container that stores the binding objects. </param>
    /// <param name="headerForControls"> A prefab that acts as a header for each section of inputs. </param>
    /// <param name="bindingObj"> A prefab that contains the binding name and a button to reassign that binding. </param>
    private void AutoGenerateBindings(SampleInput sampleInput, Transform containerForBindings, GameObject headerForControls, GameObject bindingObj)
    {
        InputAction[] allPlayerControls;

        allPlayerControls = sampleInput.Player.Get().ToArray();
        for (int i = 0; i < allPlayerControls.Length; i++)
        {
            if (allPlayerControls[i].bindings[0].isComposite)
            {
                Instantiate(headerForControls, containerForBindings).GetComponent<TMP_Text>().text = allPlayerControls[i].name;
                for (int j = 1; j < allPlayerControls[i].bindings.Count; j++)
                {
                    GameObject g = Instantiate(bindingObj, containerForBindings);
                    g.GetComponent<BindingGameObject>().FillInInformation(allPlayerControls[i], j);
                }
            }
            else
            {
                GameObject g = Instantiate(bindingObj, containerForBindings);
                g.GetComponent<BindingGameObject>().FillInInformation(allPlayerControls[i], 0);
            }
        }
    }

}
