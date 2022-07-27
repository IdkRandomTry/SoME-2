using UnityEngine;

public class Almanac : MonoBehaviour {
    public Transition transition;

    public void OnBackButtonClick() {
        transition.SwitchSceneTo("MainMenu");
    }
}
