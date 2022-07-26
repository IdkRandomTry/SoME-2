using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour {
    public Transition transition;
    public Collider2D Circle;
    public Rigidbody2D CircleRigidbody;

    public float MaxForce;
    public float StartingForce;
    public float ForceIncreaseSpeed;

    [HideInInspector]
    public bool m_pulling = false;
    [HideInInspector]
    public float m_force;

    void Start() {
        m_pulling = false;
        m_force = StartingForce;
    }

    void Update() {
        if (m_pulling) {
            Vector2 target_force = new Vector2(transform.position.x, transform.position.y) - CircleRigidbody.position;
            target_force *= m_force;
            CircleRigidbody.AddForce(target_force);
            m_force += Mathf.Min(MaxForce, Time.deltaTime * ForceIncreaseSpeed);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!m_pulling) {
            if (other == Circle) {
                m_pulling = true;
                CircleRigidbody.gravityScale = 0.0f;

                string this_scene = SceneManager.GetActiveScene().name;
                int this_scene_num = int.Parse(this_scene.Substring(5));
                string next_scene_name;
                if (this_scene_num < 9) {
                    next_scene_name = "Level0" + (this_scene_num+1);
                } else {
                    next_scene_name = "Level" + (this_scene_num+1);
                }
                PlayerProgress.latest_unlocked_level = this_scene_num + 1;

                if (Application.CanStreamedLevelBeLoaded(next_scene_name))
                    transition.SwitchSceneTo(next_scene_name);
            }
        }
    }
}
