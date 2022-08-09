using UnityEngine;

public class AlmanacNextPageButton : MonoBehaviour {
    public Animator In;
    public Animator Out;
    public Almanac AlmanacContext;
    public int NextPageNum;

    private BackgroundMusic m_background_music;

    void Start() {
        m_background_music = GameObject.Find("AudioPlayer").GetComponent<BackgroundMusic>();
    }

    public void OnClick() {
        m_background_music.click_effect_2.Play();
        if (AlmanacContext.m_currently_presenting_page != NextPageNum) {
            In.SetTrigger("In");
            Out.SetTrigger("Out");
            AlmanacContext.m_currently_presenting_page = NextPageNum;
        }
    }
}
