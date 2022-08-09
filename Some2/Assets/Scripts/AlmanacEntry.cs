using UnityEngine;

public class AlmanacEntry : MonoBehaviour {
    public Almanac content;
    public Sprite TheNewContentImage;
    public int ID;
    public bool IsSyntaxPage;

    private BackgroundMusic m_background_music;

    void Start() {
        m_background_music = GameObject.Find("AudioPlayer")?.GetComponent<BackgroundMusic>();
        if (IsSyntaxPage) {
            if (AlmanacProgress.SyntaxIsUnlocked(ID)) {
                transform.Find("Lock").gameObject.SetActive(false);

                // Center the text
                Transform text_transform = transform.Find("InnerText");
                transform.Find("InnerText").localPosition = new Vector3(0, 0, text_transform.localPosition.z);
            }
        } else {
            if (AlmanacProgress.EntryIsUnlocked(ID)) {
                transform.Find("Lock").gameObject.SetActive(false);

                // Center the text
                Transform text_transform = transform.Find("InnerText");
                transform.Find("InnerText").localPosition = new Vector3(0, 0, text_transform.localPosition.z);
            }
        }
    }

    public void OnClick() {
        m_background_music?.click_effect.Play();
        if (IsSyntaxPage) {
            if (AlmanacProgress.SyntaxIsUnlocked(ID)) {
                content.Switch(TheNewContentImage);
            }
        } else {
            if (AlmanacProgress.EntryIsUnlocked(ID)) {
                content.Switch(TheNewContentImage);
            }
        }
    }
}
