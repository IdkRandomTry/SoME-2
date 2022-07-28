using UnityEngine;

public class LevelButton : MonoBehaviour {
    public Transition transition;

    void Start() {
        if (PlayerProgress.LevelIsUnlocked(gameObject.name)) {
            transform.Find("Lock").gameObject.SetActive(false);
        }
    }

    public void OnClick() {
        if (PlayerProgress.LevelIsUnlocked(gameObject.name))
            transition.SwitchSceneTo(gameObject.name);
    }
}
