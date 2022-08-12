using UnityEngine;

public class HintButton : MonoBehaviour {
    public string HintText;

    private GameObject m_hint_object;
    private TMPro.TMP_Text m_text;

    private bool had_clicked;
    private bool is_active;

    void Start() {
        m_hint_object = transform.Find("Hint").gameObject;
        m_text = transform.Find("Hint").Find("HintText").GetComponent<TMPro.TMP_Text>();
        m_hint_object.SetActive(false);
        had_clicked = false;
        is_active = false;
    }
    
    public void OnClick() {
        if (!is_active) {
            if (!had_clicked) {
                m_hint_object.SetActive(true);
                had_clicked = true;
                m_text.text = "Are you sure you want a hint?";
            } else {
                m_hint_object.SetActive(true);
                m_text.text = HintText;
            }
            m_hint_object.SetActive(true);
            is_active = true;
        } else {
            m_hint_object.SetActive(false);
            is_active = false;
        }
    }
}
