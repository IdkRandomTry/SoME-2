using UnityEngine;

public class BackgroundMusic : MonoBehaviour {
    private AudioSource[] audio_sources;
    [HideInInspector]
    public AudioSource music;
    [HideInInspector]
    public AudioSource click_effect;
    [HideInInspector]
    public AudioSource click_effect_2;

    // Start is called before the first frame update
    void Start() {
        if (!OtherStuff.MusicPlaying) {
            DontDestroyOnLoad(gameObject);
            audio_sources = GetComponents<AudioSource>();
            music = audio_sources[0];
            click_effect = audio_sources[1];
            click_effect_2 = audio_sources[2];
            SetMusicVolume(OtherStuff.MusicVolume);
            SetSoundVolume(OtherStuff.SoundVolume);
            music.loop = true;
            music.Play();
            OtherStuff.MusicPlaying = true;
        } else Destroy(gameObject);
    }

    public void SetMusicVolume(float f) {
        music.volume = f;

        OtherStuff.MusicVolume = f;
    }

    public void SetSoundVolume(float f) {
        click_effect.volume = f;
        click_effect_2.volume = f;

        OtherStuff.SoundVolume = f;
    }
}
