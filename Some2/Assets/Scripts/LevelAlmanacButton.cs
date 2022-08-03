using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelAlmanacButton : MonoBehaviour {
    public Transition transition;

    private UnityEngine.UI.Button TheButton;

    void Start() {
        TheButton = GetComponent<UnityEngine.UI.Button>();
    }

    public void OnClick() {
        TheButton.interactable = false;
        OtherStuff.AlmanacBackButton = SceneManager.GetActiveScene().name;
        transition.SwitchSceneTo("Almanac");
    }
}
