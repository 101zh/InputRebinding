using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleMenuManager : MonoBehaviour
{
    [SerializeField] List<TMP_Text> titleScreenText;

    void Update()
    {
        titleScreenText[0].text = "Move Direction: " + InputManager.move.ReadValue<Vector2>();
        titleScreenText[1].text = "Fire is pressed: " + InputManager.fire.IsPressed();
        titleScreenText[2].text = "Jump is pressed: " + InputManager.jump.IsPressed();
        titleScreenText[3].text = "Ability 1 is pressed: " + InputManager.ability1.IsPressed();
        titleScreenText[4].text = "Ability 2 is pressed: " + InputManager.ability2.IsPressed();
    }
}
