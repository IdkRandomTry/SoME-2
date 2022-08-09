using UnityEngine;

public class RunButton : MonoBehaviour {
    public Circle[] balls;
    public Goal[] goals;

    public Sprite PlaySprite;
    public Sprite ResetSprite;

    private UnityEngine.UI.Image TheImage;

    private Vector2[] m_ball_recorded_pos;
    private Quaternion[] m_ball_recorded_rot;
    private bool m_simulating = false;

    private BackgroundMusic m_background_music;

    void Start() {
        m_background_music = GameObject.Find("AudioPlayer").GetComponent<BackgroundMusic>();

        int i = 0;
        m_ball_recorded_pos = new Vector2[balls.Length];
        m_ball_recorded_rot = new Quaternion[balls.Length];
        foreach (Circle c in balls) {
            c.Start();
            c.my_rigidbody.simulated = false;
            m_ball_recorded_pos[i] = c.transform.position;
            m_ball_recorded_rot[i] = c.transform.rotation;
            i++;
        }
        TheImage = GetComponent<UnityEngine.UI.Image>();
        TheImage.sprite = PlaySprite;
    }

    void Update() {
        foreach (Circle c in balls) {
            if (c.m_hit_a_side) OnClick();
        }
    }

    public void OnClick() {
        m_background_music.click_effect_2.Play();
        m_simulating = !m_simulating;
        
        if (m_simulating) TheImage.sprite = ResetSprite;
        else TheImage.sprite = PlaySprite;
        
        foreach (Circle c in balls) {
            c.my_rigidbody.simulated = !c.my_rigidbody.simulated;
        }

        if (!m_simulating) {
            int i = 0;
            foreach (Circle c in balls) {
                c.transform.SetPositionAndRotation(m_ball_recorded_pos[i], m_ball_recorded_rot[i]);
                c.my_rigidbody.velocity = new Vector3(0f,0f,0f); 
                c.my_rigidbody.angularVelocity = 0f;
                c.m_hit_a_side = false;
                i++;
            }

            foreach (Goal goal in goals)
                goal.StopPulling();
        }
        
    }
}
