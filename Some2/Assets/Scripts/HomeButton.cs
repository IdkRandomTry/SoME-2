using UnityEngine;

public class HomeButton : MonoBehaviour {
    public Transition transition;
    public UnityEngine.UI.Button TheButton;

    private BackgroundMusic m_background_music;

    void Start() {
        m_background_music = GameObject.Find("AudioPlayer").GetComponent<BackgroundMusic>();
    }
    
    public void OnClick() {
        TheButton.interactable = false;
        m_background_music.click_effect_2.Play();
        transition.SwitchSceneTo("MainMenu");
    }
}
