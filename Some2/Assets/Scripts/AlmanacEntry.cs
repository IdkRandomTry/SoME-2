using UnityEngine;

public class AlmanacEntry : MonoBehaviour {
    public AlmanacSelections content;
    public Sprite TheNewContentImage;

    public void OnClick() {
        content.Switch(TheNewContentImage);
    }
}
