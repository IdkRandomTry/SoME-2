using UnityEngine;

public class LevelSelect : MonoBehaviour {
    public Transition transition;

    public void OnBackButtonClick() {
        transition.SwitchSceneTo("MainMenu");
    }
}
