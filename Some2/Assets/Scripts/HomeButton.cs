using UnityEngine;

public class HomeButton : MonoBehaviour {
    public Transition transition;
    public UnityEngine.UI.Button TheButton;

    public void OnClick() {
        TheButton.interactable = false;
        transition.SwitchSceneTo("MainMenu");
    }
}
