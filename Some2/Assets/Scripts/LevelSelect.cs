using UnityEngine;

public class LevelSelect : MonoBehaviour {
    public Transition transition;
    public UnityEngine.UI.Button BackButton;

    public void OnBackButtonClick() {
        BackButton.interactable = false;
        transition.SwitchSceneTo("MainMenu");
    }
}
