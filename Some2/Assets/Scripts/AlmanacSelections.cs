using UnityEngine;

public class AlmanacSelections : MonoBehaviour {
    public Animator ImageOneAnimator;
    public Animator ImageTwoAnimator;
    public UnityEngine.UI.Image ImageOne;
    public UnityEngine.UI.Image ImageTwo;

    private Sprite last_sprite;

    // True if the currently presenting image is ImageTwo
    // False if the currently presenting image is ImageOne
    private bool currently_presenting;

    void Start() {
        ImageOneAnimator.SetTrigger("In");
        ImageTwoAnimator.SetTrigger("Out");
    }

    public void Switch(Sprite sprite) {
        if (currently_presenting) {
            ImageOne.sprite = sprite;
            ImageOne.color = new Color(1, 1, 1);
            ImageTwo.sprite = last_sprite;
            if (last_sprite != null) ImageTwo.color = new Color(1, 1, 1);
            ImageTwoAnimator.SetTrigger("Out");
            ImageOneAnimator.SetTrigger("In");
        } else {
            ImageTwo.sprite = sprite;
            ImageTwo.color = new Color(1, 1, 1);
            ImageOne.sprite = last_sprite;
            if (last_sprite != null) ImageOne.color = new Color(1, 1, 1);
            ImageOneAnimator.SetTrigger("Out");
            ImageTwoAnimator.SetTrigger("In");
        }
        last_sprite = sprite;
    }
}
