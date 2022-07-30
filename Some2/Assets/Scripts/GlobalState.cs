using UnityEngine;

public static class ApplicationLifetime {
    [RuntimeInitializeOnLoadMethod]
    public static void OnLoad() {
        PlayerProgress.latest_unlocked_level = PlayerPrefs.GetInt("LevelProgress", 1);
        AlmanacProgress.latest_unlocked_note = PlayerPrefs.GetInt("AlmanacProgress_Note", 0);
        AlmanacProgress.latest_unlocked_syntax_note = PlayerPrefs.GetInt("AlmanacProgress_Syntax", 0);
    }
}

public static class PlayerProgress {
    public static int latest_unlocked_level = 1;

    public static void UpdatePlayerPrefs() {
        PlayerPrefs.SetInt("LevelProgress", latest_unlocked_level);
    }

    public static bool LevelIsUnlocked(string s) {
        int l = int.Parse(s.Substring(5));
        return l <= latest_unlocked_level;
    }

    public static string CurrentSceneName() {
        string next_scene_name;
        if (latest_unlocked_level < 9) {
            next_scene_name = "Level0" + (latest_unlocked_level);
        } else {
            next_scene_name = "Level" + (latest_unlocked_level);
        }
        return next_scene_name;
    }
}

public static class AlmanacProgress {
    public static int latest_unlocked_note = 0;
    public static int latest_unlocked_syntax_note = 0;

    public static void UpdatePlayerPrefs() {
        Debug.Log("Call " + latest_unlocked_note + "  " + latest_unlocked_syntax_note);
        PlayerPrefs.SetInt("AlmanacProgress_Note", latest_unlocked_note);
        PlayerPrefs.SetInt("AlmanacProgress_Syntax", latest_unlocked_syntax_note);
    }

    public static bool EntryIsUnlocked(int id) {
        return id <= latest_unlocked_note;
    }

    public static bool SyntaxIsUnlocked(int id) {
        return id <= latest_unlocked_syntax_note;
    }
}