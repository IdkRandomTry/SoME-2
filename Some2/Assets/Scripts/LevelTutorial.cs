using UnityEngine;

public class Stage {
    public Vector2 position;
    public Vector2 scale;
    public string text;

    public Stage(Vector2 position, Vector2 scale, string text) {
        this.position = position;
        this.scale = scale;
        this.text = text;
    }
}

public class LevelTutorial : MonoBehaviour {
    private int index;
    private Stage[] TutorialDialogueStages;
    public UnityEngine.UI.Image DialogueBox;
    public TMPro.TMP_Text DialogueBoxText;

    void Start() {
        index = 0;
        TutorialDialogueStages = new Stage[] {
            new Stage(new Vector2(0, 0), new Vector2(170, 60), "Welcome to Graph it"),
            new Stage(new Vector2(225.4f, 102.7f), new Vector2(170, 60), "You have to get the Ball..."),
            new Stage(new Vector2(-28.1f, 0), new Vector2(170, 60), "... To the goal..."),
            new Stage(new Vector2(-255, 49.5f), new Vector2(170, 60), "... Using only graphs"),
            new Stage(new Vector2(-255, 110.8f), new Vector2(170, 60), "Here's a graph that you cannot change"),
            new Stage(new Vector2(-255, 44.3f), new Vector2(170, 60), "Here's a graph that you can change"),
            new Stage(new Vector2(-255, 44.3f), new Vector2(170, 60), "Input \"x\" here and press enter"),
            new Stage(new Vector2(0, 0), new Vector2(170, 60), "Click the play button"),
            new Stage(new Vector2(0, 0), new Vector2(170, 60), "And let the physics do the rest!"),
        };
        Stage s = TutorialDialogueStages[index];
        DialogueBox.rectTransform.anchoredPosition = new Vector3(s.position.x, s.position.y, DialogueBox.rectTransform.position.z);
        DialogueBox.rectTransform.sizeDelta = s.scale;
        DialogueBoxText.text = s.text;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) {
            if (index + 1 == TutorialDialogueStages.Length) {
                Debug.Log("Beepbeep");
                DialogueBox.rectTransform.anchoredPosition = new Vector3(0, 0, DialogueBox.rectTransform.position.z);
                DialogueBox.rectTransform.sizeDelta = new Vector2(0, 0);
                DialogueBoxText.text = "";
                return;
            }

            Stage s = TutorialDialogueStages[++index];
            DialogueBox.rectTransform.anchoredPosition = new Vector3(s.position.x, s.position.y, DialogueBox.rectTransform.position.z);
            DialogueBox.rectTransform.sizeDelta = s.scale;
            DialogueBoxText.text = s.text;
        }
    }
}

