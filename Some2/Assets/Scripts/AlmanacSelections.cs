using UnityEngine;

public class AlmanacSelections : MonoBehaviour {
    private CanvasGroup m_canvas_group;

    void Start() {
        m_canvas_group = GetComponent<CanvasGroup>();
    }

    public void ChangeInteractibilityFalse(AnimationEvent e) {
        m_canvas_group.interactable = false;
    }
    
    public void ChangeInteractibilityTrue(AnimationEvent e) {
        m_canvas_group.interactable = true;
    }
}
