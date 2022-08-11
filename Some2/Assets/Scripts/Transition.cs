using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour {
    private Animator[] animators;
    // private Animator entry_unlocked_animator;
    public float transition_time = 0.8f;
    public float image_move_time = 0.8f;

    private UnityEngine.UI.Image PresentableImage;
    private Animator PresentingImageAnimator;
    private Animator PresentingTextAnimator;

    public GameObject ImageObject;

    [System.Obsolete]
    public void Start() {
        gameObject.SetActiveRecursively(true);
        animators = new Animator[5];
        animators[0] = transform.Find("Top").GetComponent<Animator>();
        animators[1] = transform.Find("TopM").GetComponent<Animator>();
        animators[2] = transform.Find("Mid").GetComponent<Animator>();
        animators[3] = transform.Find("BotM").GetComponent<Animator>();
        animators[4] = transform.Find("Bot").GetComponent<Animator>();
        
        if (ImageObject != null) {
            PresentableImage = ImageObject?.GetComponent<UnityEngine.UI.Image>();
            PresentableImage.color = Color.white;
            PresentingImageAnimator = ImageObject?.GetComponent<Animator>();
        }
        GameObject UnlockTextObject = transform.Find("UnlockText")?.gameObject;
        PresentingTextAnimator = UnlockTextObject?.GetComponent<Animator>();

        foreach (Animator a in animators) a.enabled = true;

        if (OtherStuff.WasEntryUnlockedForTransition) {
            OtherStuff.NoteEntriesUnlocked.Clear();
            OtherStuff.SyntaxEntriesUnlocked.Clear();
            OtherStuff.WasEntryUnlockedForTransition = false;
        }
    }

    public void SwitchSceneTo(string next) {
        StartCoroutine("LoadLevel", next);
    }

    IEnumerator LoadLevel(string next) {
        AsyncOperation task = SceneManager.LoadSceneAsync(next);
        task.allowSceneActivation = false;
        foreach (Animator a in animators)
            a.SetTrigger("TransitionIn");
        yield return new WaitForSeconds(transition_time);

        if (OtherStuff.WasEntryUnlockedForTransition) {
            PresentingTextAnimator.SetTrigger("In");
            foreach (int idx in OtherStuff.NoteEntriesUnlocked) {
                foreach (Sprite s in AlmanacSpriteRegistry.NotesSprites[idx]) {
                    PresentableImage.sprite = s;
                    PresentingImageAnimator.SetTrigger("In");
                    yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2));
                    PresentingImageAnimator.SetTrigger("Out");
                    yield return new WaitForSeconds(image_move_time);
                }
            }
            foreach (int idx in OtherStuff.SyntaxEntriesUnlocked) {
                foreach (Sprite s in AlmanacSpriteRegistry.SyntaxNotesSprites[idx]) {
                    PresentableImage.sprite = s;
                    PresentingImageAnimator.SetTrigger("In");
                    yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2));
                    PresentingImageAnimator.SetTrigger("Out");
                    yield return new WaitForSeconds(image_move_time);
                }
            }
            PresentingTextAnimator.SetTrigger("Out");
            yield return new WaitForSeconds(image_move_time);
        }

        task.allowSceneActivation = true;
    }
}
