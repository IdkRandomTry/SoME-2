using UnityEngine;

public class MainMenu : MonoBehaviour {
    public Transition transition;

    public void OnStartButtonClick() {
        transition.SwitchSceneTo("Level01");
    }

    public void OnAlmanacButtonClick() {
        Debug.Log("Almanac Open");
    }

    public void OnLevelSelectButtonClick() {
        transition.SwitchSceneTo("LevelSelect");
    }

    public void OnQuitButtonClick() {
        Application.Quit(0);
    }
}
