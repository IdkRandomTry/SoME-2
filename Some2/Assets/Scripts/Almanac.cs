using UnityEngine;

public class Almanac : MonoBehaviour {
    public Transition transition;

    public Animator ImageOneAnimator;
    public Animator ImageTwoAnimator;
    public UnityEngine.UI.Image ImageOne;
    public UnityEngine.UI.Image ImageTwo;
    public UnityEngine.UI.Button BackButton;

    private Sprite m_last_sprite;

    [HideInInspector]
    public int m_currently_presenting_page;

    // True if the currently presenting image is ImageTwo
    // False if the currently presenting image is ImageOne
    private bool m_currently_presenting_image;
    private BackgroundMusic m_background_music;

    void Start() {
        m_background_music = GameObject.Find("AudioPlayer")?.GetComponent<BackgroundMusic>();
        m_currently_presenting_page = 0;
    }

    public void Switch(Sprite sprite) {
        if (m_last_sprite != sprite) {
            if (m_currently_presenting_image) {
                ImageOne.sprite = sprite;
                ImageOne.color = new Color(1, 1, 1);
                ImageTwoAnimator.SetTrigger("Out");
                ImageOneAnimator.SetTrigger("In");
            } else {
                ImageTwo.sprite = sprite;
                ImageTwo.color = new Color(1, 1, 1);
                ImageOneAnimator.SetTrigger("Out");
                ImageTwoAnimator.SetTrigger("In");
            }
            m_last_sprite = sprite;
            m_currently_presenting_image = !m_currently_presenting_image;
        }
    }
    
    public void OnBackButtonClick() {
        BackButton.interactable = false;
        m_background_music?.click_effect.Play();
        transition.SwitchSceneTo(OtherStuff.AlmanacBackButton);
    }
}
