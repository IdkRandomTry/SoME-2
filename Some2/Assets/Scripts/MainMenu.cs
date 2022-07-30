using UnityEngine;

public class MainMenu : MonoBehaviour {
    public Transition transition;

    public void OnStartButtonClick() {
        if (Application.CanStreamedLevelBeLoaded(PlayerProgress.CurrentSceneName())) {
            transition.SwitchSceneTo(PlayerProgress.CurrentSceneName());
        } else transition.SwitchSceneTo("Level01");
    }

    public void OnAlmanacButtonClick() {
        transition.SwitchSceneTo("Almanac");
    }

    public void OnLevelSelectButtonClick() {
        transition.SwitchSceneTo("LevelSelect");
    }

    public void OnQuitButtonClick() {
        Application.Quit(0);
    }
}
