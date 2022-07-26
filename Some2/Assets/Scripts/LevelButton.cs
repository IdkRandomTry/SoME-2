using UnityEngine;

public class LevelButton : MonoBehaviour {
    public Transition transition;
    public string level;

    private GameObject lock_object;

    void Start() {
        if (PlayerProgress.LevelIsUnlocked(level)) {
            lock_object = transform.Find("Lock").gameObject;
            lock_object.SetActive(false);
        }
    }

    public void OnClick() {
        if (PlayerProgress.LevelIsUnlocked(level))
            transition.SwitchSceneTo(level);
    }
}
