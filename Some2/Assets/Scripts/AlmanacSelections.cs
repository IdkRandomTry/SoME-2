using UnityEngine;

public class AlmanacSelections : MonoBehaviour {
    public Animator ImageOneAnimator;
    public Animator ImageTwoAnimator;
    public UnityEngine.UI.Image ImageOne;
    public UnityEngine.UI.Image ImageTwo;

    private Sprite m_last_sprite;
    private CanvasGroup m_canvas_group;

    // True if the currently presenting image is ImageTwo
    // False if the currently presenting image is ImageOne
    private bool m_currently_presenting_image;

    void Start() {
        ImageOneAnimator.SetTrigger("In");
        ImageTwoAnimator.SetTrigger("Out");

        m_canvas_group = GetComponent<CanvasGroup>();
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

    public void ChangeInteractibilityFalse(AnimationEvent e) {
        m_canvas_group.interactable = false;
    }
    
    public void ChangeInteractibilityTrue(AnimationEvent e) {
        m_canvas_group.interactable = true;
    }
}
