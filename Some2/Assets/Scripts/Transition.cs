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
    private Animator ContinueButtonAnimator;

    public GameObject ImageObject;

    private GameObject ContinueButton;
    private bool ClickedContinue;

    private BackgroundMusic m_background_music;

    [System.Obsolete]
    public void Start() {
        gameObject.SetActiveRecursively(true);

        m_background_music = GameObject.Find("AudioPlayer")?.GetComponent<BackgroundMusic>();
        animators = new Animator[5];
        animators[0] = transform.Find("Top").GetComponent<Animator>();
        animators[1] = transform.Find("TopM").GetComponent<Animator>();
        animators[2] = transform.Find("Mid").GetComponent<Animator>();
        animators[3] = transform.Find("BotM").GetComponent<Animator>();
        animators[4] = transform.Find("Bot").GetComponent<Animator>();

        transform.Find("Top").GetComponent<UnityEngine.UI.Image>().color = new Color(44.0f/255.0f, 44.0f/255.0f, 44.0f/255.0f);
        transform.Find("TopM").GetComponent<UnityEngine.UI.Image>().color = new Color(44.0f/255.0f, 44.0f/255.0f, 44.0f/255.0f);
        transform.Find("Mid").GetComponent<UnityEngine.UI.Image>().color = new Color(44.0f/255.0f, 44.0f/255.0f, 44.0f/255.0f);
        transform.Find("BotM").GetComponent<UnityEngine.UI.Image>().color = new Color(44.0f/255.0f, 44.0f/255.0f, 44.0f/255.0f);
        transform.Find("Bot").GetComponent<UnityEngine.UI.Image>().color = new Color(44.0f/255.0f, 44.0f/255.0f, 44.0f/255.0f);
        
        if (ImageObject != null) {
            PresentableImage = ImageObject?.GetComponent<UnityEngine.UI.Image>();
            PresentableImage.color = Color.white;
            PresentingImageAnimator = ImageObject?.GetComponent<Animator>();
        }
        GameObject UnlockTextObject = transform.Find("UnlockText")?.gameObject;
        PresentingTextAnimator = UnlockTextObject?.GetComponent<Animator>();
        
        ContinueButton = transform.Find("ContinueButton")?.gameObject;
        ContinueButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        ContinueButtonAnimator = ContinueButton.GetComponent<Animator>();
        ContinueButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
            m_background_music?.click_effect_2.Play();
            ClickedContinue = true;
        });

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
        ContinueButton.SetActive(true);
        AsyncOperation task = SceneManager.LoadSceneAsync(next);
        task.allowSceneActivation = false;
        foreach (Animator a in animators)
            a.SetTrigger("TransitionIn");
        yield return new WaitForSeconds(transition_time);

        if (OtherStuff.WasEntryUnlockedForTransition) {
            PresentingTextAnimator.SetTrigger("In");
            ContinueButtonAnimator.SetTrigger("In");
            foreach (int idx in OtherStuff.NoteEntriesUnlocked) {
                foreach (Sprite s in AlmanacSpriteRegistry.NotesSprites[idx]) {
                    PresentableImage.sprite = s;
                    PresentingImageAnimator.SetTrigger("In");
                    yield return new WaitUntil(() => {
                        if (ClickedContinue) {
                            ClickedContinue = false;
                            return true;
                        }
                        return false;
                    });
                    PresentingImageAnimator.SetTrigger("Out");
                    yield return new WaitForSeconds(image_move_time);
                }
            }
            foreach (int idx in OtherStuff.SyntaxEntriesUnlocked) {
                foreach (Sprite s in AlmanacSpriteRegistry.SyntaxNotesSprites[idx]) {
                    PresentableImage.sprite = s;
                    PresentingImageAnimator.SetTrigger("In");
                    yield return new WaitUntil(() => {
                        if (ClickedContinue) {
                            ClickedContinue = false;
                            return true;
                        }
                        return false;
                    });
                    PresentingImageAnimator.SetTrigger("Out");
                    yield return new WaitForSeconds(image_move_time);
                }
            }
            PresentingTextAnimator.SetTrigger("Out");
            ContinueButtonAnimator.SetTrigger("Out");
            yield return new WaitForSeconds(image_move_time);
        }

        task.allowSceneActivation = true;
    }
}
