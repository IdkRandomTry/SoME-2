using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelAlmanacButton : MonoBehaviour {
    public Transition transition;

    private UnityEngine.UI.Button TheButton;
    private BackgroundMusic m_background_music;

    void Start() {
        m_background_music = GameObject.Find("AudioPlayer")?.GetComponent<BackgroundMusic>();
        TheButton = GetComponent<UnityEngine.UI.Button>();
    }

    public void OnClick() {
        m_background_music?.click_effect_2.Play();
        TheButton.interactable = false;
        OtherStuff.AlmanacBackButton = SceneManager.GetActiveScene().name;
        transition.SwitchSceneTo("Almanac");
    }
}
