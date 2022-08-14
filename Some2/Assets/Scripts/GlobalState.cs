using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class ApplicationLifetime {
    [RuntimeInitializeOnLoadMethod]
    public static void OnLoad() {
        PlayerProgress.latest_unlocked_level = PlayerPrefs.GetInt("LevelProgress", 1);
        AlmanacProgress.latest_unlocked_note = PlayerPrefs.GetInt("AlmanacProgress_Note", -1);
        AlmanacProgress.latest_unlocked_syntax_note = PlayerPrefs.GetInt("AlmanacProgress_Syntax", -1);
        OtherStuff.PlayedTutorial = PlayerPrefs.GetInt("Tutorial_Played", 0) == 0 ? false : true;
        AlmanacSpriteRegistry.Load();

        File.WriteAllText("test.txt", "" + PlayerPrefs.GetInt("Tutorial_Played", 0));
    }
}

public static class OtherStuff {
    public static bool WasEntryUnlockedForTransition = false;
    public static List<int> SyntaxEntriesUnlocked = new List<int>();
    public static List<int> NoteEntriesUnlocked = new List<int>();
    public static bool PlayedTutorial;
    public static string AlmanacBackButton = "MainMenu";
    public static bool MusicPlaying = false;

    public static float MusicVolume = 0.65f;
    public static float SoundVolume = 1.0f;

    public static void UpdatePlayerPrefs() {
        PlayerPrefs.SetInt("Tutorial_Played", PlayedTutorial ? 1 : 0);
    }
}

public static class AlmanacSpriteRegistry {
    public static Dictionary<int, Sprite[]> NotesSprites;
    public static Dictionary<int, Sprite[]> SyntaxNotesSprites;

    public static void Load() {
        if (NotesSprites == null) {
            NotesSprites = new Dictionary<int, Sprite[]>();
            NotesSprites[1] = new Sprite[] { Resources.Load<Sprite>("Almanac Entries/slope") };
            NotesSprites[2] = new Sprite[] { Resources.Load<Sprite>("Almanac Entries/shift of origin") };
            NotesSprites[3] = new Sprite[] { Resources.Load<Sprite>("Almanac Entries/X-scaling") };
            NotesSprites[4] = new Sprite[] { Resources.Load<Sprite>("Almanac Entries/Y-scaling") };
            NotesSprites[5] = new Sprite[] { Resources.Load<Sprite>("Almanac Entries/X-reflection") };
            NotesSprites[6] = new Sprite[] { Resources.Load<Sprite>("Almanac Entries/Y-reflection") };
            NotesSprites[7] = new Sprite[] { Resources.Load<Sprite>("Almanac Entries/mod(f(x))") };
            NotesSprites[8] = new Sprite[] { Resources.Load<Sprite>("Almanac Entries/f(mod(x))") };
        }
        if (SyntaxNotesSprites == null) {
            SyntaxNotesSprites = new Dictionary<int, Sprite[]>();
            SyntaxNotesSprites[0] = new Sprite[] {
                Resources.Load<Sprite>("Almanac Entries/rules"),
                Resources.Load<Sprite>("Almanac Entries/syntax")
            };
            SyntaxNotesSprites[2] = new Sprite[] {
                Resources.Load<Sprite>("Almanac Entries/trig help"),
                Resources.Load<Sprite>("Almanac Entries/const pi"),
            };
            SyntaxNotesSprites[3] = new Sprite[] { Resources.Load<Sprite>("Almanac Entries/min help") };
            SyntaxNotesSprites[4] = new Sprite[] { Resources.Load<Sprite>("Almanac Entries/max help") };
            SyntaxNotesSprites[5] = new Sprite[] { Resources.Load<Sprite>("Almanac Entries/log help") };
            SyntaxNotesSprites[6] = new Sprite[] { Resources.Load<Sprite>("Almanac Entries/mod(x) help") };
        }
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
    public static int latest_unlocked_note = -1;
    public static int latest_unlocked_syntax_note = -1;

    public static void UpdatePlayerPrefs() {
        Debug.Log("Call " + latest_unlocked_note + "  " + latest_unlocked_syntax_note);
        PlayerPrefs.SetInt("AlmanacProgress_Note", latest_unlocked_note);
        PlayerPrefs.SetInt("AlmanacProgress_Syntax", latest_unlocked_syntax_note);
    }

    public static bool EntryIsUnlocked(int id) {
        return id <= latest_unlocked_note;
    }

    public static bool SyntaxIsUnlocked(int id) {
        Debug.Log(id);
        return id <= latest_unlocked_syntax_note;
    }
}