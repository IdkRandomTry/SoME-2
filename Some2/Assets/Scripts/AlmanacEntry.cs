using UnityEngine;

public class AlmanacEntry : MonoBehaviour {
    public UnityEngine.UI.Image content;
    public Sprite TheNewContentImage;

    public void OnClick() {
        content.sprite = TheNewContentImage;
        content.color = new Color(1, 1, 1);
    }
}
