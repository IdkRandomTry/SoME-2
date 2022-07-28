using UnityEngine;

public class TextDefault : MonoBehaviour {
    public string DefaultText;
    
    void Start() {
        GetComponent<TMPro.TMP_InputField>().text = DefaultText;
    }
}
