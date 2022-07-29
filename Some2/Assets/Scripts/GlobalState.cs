using UnityEngine;

public static class ApplicationLifetime {
    [RuntimeInitializeOnLoadMethod]
    public static void OnLoad() {
        PlayerProgress.latest_unlocked_level = PlayerPrefs.GetInt("LevelProgress", 1);
        AlmanacProgress.latest_unlocked_entry = PlayerPrefs.GetInt("AlmanacProgress", 0);
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
}

public static class AlmanacProgress {
    public static int latest_unlocked_entry = 0;

    public static void UpdatePlayerPrefs() {
        PlayerPrefs.SetInt("AlmanacProgress", latest_unlocked_entry);
    }

    public static bool EntryIsUnlocked(int id) {
        return id <= latest_unlocked_entry;
    }
}