using UnityEngine;
using TMPro;

public class RunButton : MonoBehaviour {
    public GameObject ball;

    private Vector2 m_ball_recorded_pos;
    private Quaternion m_ball_recorded_rot;
    private Rigidbody2D m_ball_rigidbody;
    private bool m_simulating = false;

    void Start() {
        m_ball_rigidbody = ball.GetComponent<Rigidbody2D>();
        m_ball_rigidbody.simulated = false;
        m_ball_recorded_pos = ball.transform.position;
        m_ball_recorded_rot = ball.transform.rotation;
    }

    public void OnClick() {
        m_simulating = !m_simulating;
        m_ball_rigidbody.simulated = !m_ball_rigidbody.simulated;

        if (!m_simulating) {
            ball.transform.SetPositionAndRotation(m_ball_recorded_pos, m_ball_recorded_rot);
            m_ball_rigidbody.velocity = new Vector3(0f,0f,0f); 
            m_ball_rigidbody.angularVelocity = 0f;
        }
        
    }
}
