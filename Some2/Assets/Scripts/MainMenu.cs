using UnityEngine;

public class MainMenu : MonoBehaviour {
    public Transition transition;
    public UnityEngine.UI.Button StartButton;
    public UnityEngine.UI.Button AlmanacButton;
    public UnityEngine.UI.Button LevelSelectButton;
    public UnityEngine.UI.Button QuitButton;

    void Start() {
        if (!OtherStuff.PlayedTutorial)
            transition.SwitchSceneTo("Tutorial");
    }

    public void OnStartButtonClick() {
        StartButton.interactable = false;
        if (Application.CanStreamedLevelBeLoaded(PlayerProgress.CurrentSceneName())) {
            transition.SwitchSceneTo(PlayerProgress.CurrentSceneName());
        } else transition.SwitchSceneTo("Level01");
    }

    public void OnAlmanacButtonClick() {
        AlmanacButton.interactable = false;
        OtherStuff.AlmanacBackButton = "MainMenu";
        transition.SwitchSceneTo("Almanac");
    }

    public void OnLevelSelectButtonClick() {
        LevelSelectButton.interactable = false;
        transition.SwitchSceneTo("LevelSelect");
    }

    public void OnQuitButtonClick() {
        QuitButton.interactable = false;
        Application.Quit(0);
    }
}
