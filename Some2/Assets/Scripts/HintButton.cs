using UnityEngine;

public class HintButton : MonoBehaviour {
    public string HintText;

    public TMPro.TMP_InputField[] InputFields;
    public FunctionInput[] FunctionInputs;
    public string[] SolutionTexts;

    private GameObject m_hint_object;
    private GameObject m_show_solution;
    private GameObject m_hint_arrows;
    private GameObject m_reclick_arrow;
    private TMPro.TMP_Text m_text;
    private UnityEngine.UI.Button m_show_solution_button;

    private bool had_clicked;
    private bool is_active;

    void Start() {
        m_hint_object = transform.Find("Hint").gameObject;
        m_hint_arrows = transform.Find("Hint").Find("HintArrows").gameObject;
        m_reclick_arrow = transform.Find("Hint").Find("ArrowBob").gameObject;
        m_show_solution = transform.Find("Hint").Find("ShowSolution").gameObject;
        m_show_solution_button = m_show_solution.GetComponent<UnityEngine.UI.Button>();
        m_show_solution_button.interactable = false;
        m_show_solution.SetActive(false);

        m_text = transform.Find("Hint").Find("HintText").GetComponent<TMPro.TMP_Text>();
        m_hint_object.SetActive(false);
        had_clicked = false;
        is_active = false;

        Debug.Assert(InputFields.Length == SolutionTexts.Length);
    }
    
    public void OnClick() {
        if (!is_active) {
            if (!had_clicked) {
                m_hint_object.SetActive(true);
                had_clicked = true;
                m_show_solution_button.interactable = false;
                m_show_solution.SetActive(false);
                m_text.text = "Are you sure you want a hint? \nClick again to reveal hint";
                m_hint_arrows.SetActive(false);
            } else {
                m_hint_object.SetActive(true);
                m_text.text = HintText;
                m_show_solution_button.interactable = true;
                m_show_solution.SetActive(true);
                m_hint_arrows.SetActive(true);
                m_reclick_arrow.SetActive(false);
            }
            m_hint_object.SetActive(true);
            is_active = true;
        } else {
            m_show_solution_button.interactable = false;
            m_show_solution.SetActive(false);
            m_hint_object.SetActive(false);
            m_hint_arrows.SetActive(false);
            is_active = false;
        }
    }

    public void SolutionButtonClick() {
        for (int i = 0; i < SolutionTexts.Length; i++) {
            InputFields[i].text = SolutionTexts[i];
        }

        for (int i = 0; i < FunctionInputs.Length; i++) {
            FunctionInputs[i].StringPing(null);
        }

        m_show_solution_button.interactable = false;
        m_show_solution.SetActive(false);
        m_hint_object.SetActive(false);
        m_hint_arrows.SetActive(false);
        is_active = false;
    }
}
