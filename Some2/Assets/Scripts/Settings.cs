using UnityEngine;

public class Settings : MonoBehaviour {
    public Transition transition;
    public UnityEngine.UI.Button BackButton;

    public UnityEngine.UI.Slider MusicSlider;
    public UnityEngine.UI.Slider SoundSlider;

    private BackgroundMusic m_background_music;

    void Start() {
        m_background_music = GameObject.Find("AudioPlayer").GetComponent<BackgroundMusic>();
        MusicSlider.value = OtherStuff.MusicVolume;
        SoundSlider.value = OtherStuff.SoundVolume;
    }

    public void OnBackButtonClick() {
        m_background_music.click_effect.Play();
        BackButton.interactable = false;
        transition.SwitchSceneTo("MainMenu");
    }

    public void OnMusicValueChanged(float value) {
        m_background_music?.SetMusicVolume(value);
    }

    public void OnSoundValueChanged(float value) {
        m_background_music?.SetSoundVolume(value);
    }
}
