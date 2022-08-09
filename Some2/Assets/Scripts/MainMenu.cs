using UnityEngine;

public class MainMenu : MonoBehaviour {
    public Transition transition;
    public UnityEngine.UI.Button StartButton;
    public UnityEngine.UI.Button AlmanacButton;
    public UnityEngine.UI.Button LevelSelectButton;
    public UnityEngine.UI.Button QuitButton;
    public UnityEngine.UI.Button SettingsButton;

    private bool initted = false;
    
    private BackgroundMusic m_background_music;

    void Start() {
        m_background_music = GameObject.Find("AudioPlayer")?.GetComponent<BackgroundMusic>();
    }

    void Update() {
        if (!initted) {
            if (!OtherStuff.PlayedTutorial) {
                transition.SwitchSceneTo("Level00");
            }
            initted = true;
        }
    }

    public void OnStartButtonClick() {
        StartButton.interactable = false;
        m_background_music?.click_effect.Play();
        if (Application.CanStreamedLevelBeLoaded(PlayerProgress.CurrentSceneName())) {
            transition.SwitchSceneTo(PlayerProgress.CurrentSceneName());
        } else transition.SwitchSceneTo("Level01");
    }

    public void OnAlmanacButtonClick() {
        AlmanacButton.interactable = false;
        m_background_music?.click_effect.Play();
        OtherStuff.AlmanacBackButton = "MainMenu";
        transition.SwitchSceneTo("Almanac");
    }

    public void OnLevelSelectButtonClick() {
        LevelSelectButton.interactable = false;
        m_background_music?.click_effect.Play();
        transition.SwitchSceneTo("LevelSelect");
    }

    public void OnQuitButtonClick() {
        QuitButton.interactable = false;
        m_background_music?.click_effect.Play();
        Application.Quit(0);
    }

    public void OnSettingsButtonClick() {
        SettingsButton.interactable = false;
        m_background_music.click_effect.Play();
        transition.SwitchSceneTo("Settings");
    }
}
