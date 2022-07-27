using UnityEngine;

public static class PlayerProgress {
    public static int latest_unlocked_level = 1;

    public static bool LevelIsUnlocked(string s) {
        int l = int.Parse(s.Substring(5));
        return l <= latest_unlocked_level;
    }
}

public static class AlmanacProgress {
    public static int latest_unlocked_entry = 1;
    // Set by AlmanacEntry.cs
    public static GameObject last_open_entry;

    public static bool EntryIsUnlocked(int id) {
        return id <= latest_unlocked_entry;
    }
}