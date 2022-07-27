using UnityEngine;

public class LevelButton : MonoBehaviour {
    public Transition transition;

    private GameObject lock_object;

    void Start() {
        if (PlayerProgress.LevelIsUnlocked(gameObject.name)) {
            lock_object = transform.Find("Lock").gameObject;
            lock_object.SetActive(false);
        }
    }

    public void OnClick() {
        if (PlayerProgress.LevelIsUnlocked(gameObject.name))
            transition.SwitchSceneTo(gameObject.name);
    }
}
