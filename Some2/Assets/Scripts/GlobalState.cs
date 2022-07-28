using UnityEngine;

public static class PlayerProgress {
    public static int latest_unlocked_level = 1;

    public static bool LevelIsUnlocked(string s) {
        int l = int.Parse(s.Substring(5));
        return l <= latest_unlocked_level;
    }
}

public static class AlmanacProgress {
    public static int latest_unlocked_entry = 2;

    public static bool EntryIsUnlocked(int id) {
        return id <= latest_unlocked_entry;
    }
}