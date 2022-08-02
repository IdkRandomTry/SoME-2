using UnityEngine;

public class LevelButton : MonoBehaviour {
    public Transition transition;
    private UnityEngine.UI.Button TheButton;

    void Start() {
        if (PlayerProgress.LevelIsUnlocked(gameObject.name)) {
            transform.Find("Lock").gameObject.SetActive(false);
        }

        TheButton = GetComponent<UnityEngine.UI.Button>();
    }

    public void OnClick() {
        if (PlayerProgress.LevelIsUnlocked(gameObject.name)) {
            TheButton.interactable = false;
            transition.SwitchSceneTo(gameObject.name);
        }
    }
}
