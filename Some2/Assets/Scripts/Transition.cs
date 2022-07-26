using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour {
    private Animator[] animators;
    public float transition_time = 5.0f;

    [System.Obsolete]
    void Start() {
        gameObject.SetActiveRecursively(true);
        animators = GetComponentsInChildren<Animator>();
    }

    public void SwitchSceneTo(string next) {
        StartCoroutine(LoadLevel(next));
    }

    IEnumerator LoadLevel(string next) {
        foreach (Animator a in animators)
            a.SetTrigger("TransitionIn");
        AsyncOperation task = SceneManager.LoadSceneAsync(next);
        task.allowSceneActivation = false;
        yield return new WaitForSeconds(transition_time);
        task.allowSceneActivation = true;
    }
}
