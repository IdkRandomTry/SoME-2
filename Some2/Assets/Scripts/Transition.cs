using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour {
    private Animator[] animators;
    private Animator entry_unlocked_animator;
    public float transition_time = 5.0f;

    [System.Obsolete]
    public void Start() {
        gameObject.SetActiveRecursively(true);
        animators = new Animator[5];
        animators[0] = transform.Find("Top").GetComponent<Animator>();
        animators[1] = transform.Find("TopM").GetComponent<Animator>();
        animators[2] = transform.Find("Mid").GetComponent<Animator>();
        animators[3] = transform.Find("BotM").GetComponent<Animator>();
        animators[4] = transform.Find("Bot").GetComponent<Animator>();
        entry_unlocked_animator = transform.Find("Intermediate").Find("Mask").GetComponent<Animator>();
        foreach (Animator a in animators) a.enabled = true;

        if (OtherStuff.WasEntryUnlockedForTransition) {
            entry_unlocked_animator.SetTrigger("Out");
            OtherStuff.WasEntryUnlockedForTransition = false;
        }
    }

    public void SwitchSceneTo(string next) {
        StartCoroutine("LoadLevel", next);
    }

    IEnumerator LoadLevel(string next) {
        foreach (Animator a in animators)
            a.SetTrigger("TransitionIn");
        AsyncOperation task = SceneManager.LoadSceneAsync(next);
        task.allowSceneActivation = false;
        yield return new WaitForSeconds(transition_time);
        if (OtherStuff.WasEntryUnlockedForTransition) {
            entry_unlocked_animator.SetTrigger("In");
            yield return new WaitForSeconds(transition_time);
        }

        task.allowSceneActivation = true;
    }
}
