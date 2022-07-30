using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour {
    public Circle[] AllCircles;

    public Transition transition;
    public int UnlockedAlmanacEntry = -1; // -1 is default for no entry unlock
    public int UnlockedAlmanacSyntaxEntry = -1; // -1 is default for no entry unlock
    private bool m_is_transitioning;

    void Start() {
        m_is_transitioning = false;
    }

    void Update() {
        if (!m_is_transitioning) {
            foreach (Circle c in AllCircles) {
                if (!c.m_found_a_goal) return;
            }
            AllGoalsActive();
            m_is_transitioning = true;
        }
    }

    void AllGoalsActive() {
        string this_scene = SceneManager.GetActiveScene().name;
        int this_scene_num = int.Parse(this_scene.Substring(5));
        string next_scene_name;
        if (this_scene_num < 9) {
            next_scene_name = "Level0" + (this_scene_num+1);
        } else {
            next_scene_name = "Level" + (this_scene_num+1);
        }
        PlayerProgress.latest_unlocked_level = this_scene_num + 1;
        PlayerProgress.UpdatePlayerPrefs();

        if (!AlmanacProgress.EntryIsUnlocked(UnlockedAlmanacEntry)) {
            AlmanacProgress.latest_unlocked_note = UnlockedAlmanacEntry;
            AlmanacProgress.UpdatePlayerPrefs();
            OtherStuff.WasEntryUnlockedForTransition = true;
        }
        if (!AlmanacProgress.SyntaxIsUnlocked(UnlockedAlmanacSyntaxEntry)) {
            AlmanacProgress.latest_unlocked_syntax_note = UnlockedAlmanacSyntaxEntry;
            AlmanacProgress.UpdatePlayerPrefs();
            OtherStuff.WasEntryUnlockedForTransition = true;
        }

        if (Application.CanStreamedLevelBeLoaded(next_scene_name)) {
            transition.SwitchSceneTo(next_scene_name);
        }
    }
}
