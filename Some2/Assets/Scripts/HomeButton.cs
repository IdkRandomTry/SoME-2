using UnityEngine;

public class HomeButton : MonoBehaviour {
    public Transition transition;

    public void OnClick() {
        transition.SwitchSceneTo("MainMenu");
    }
}
