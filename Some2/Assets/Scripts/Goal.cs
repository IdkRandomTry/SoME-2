using UnityEngine;

public class Goal : MonoBehaviour {
    public Circle[] CirclesAllowed;

    public float MaxForce;
    public float StartingForce;
    public float ForceIncreaseSpeed;

    [HideInInspector]
    public bool[] m_pulling_circles;
    [HideInInspector]
    public float[] m_forces_on_circles;

    void Start() {
        m_pulling_circles = new bool[CirclesAllowed.Length];
        m_forces_on_circles = new float[CirclesAllowed.Length];

        int i = 0;
        foreach (Circle c in CirclesAllowed) {
            m_pulling_circles[i] = false;
            m_forces_on_circles[i] = StartingForce;
            i++;
        }
    }

    void Update() {
        for (int i = 0; i < CirclesAllowed.Length; i++) {
            if (m_pulling_circles[i]) {
                Vector2 target_force = new Vector2(transform.position.x, transform.position.y) - CirclesAllowed[i].my_rigidbody.position;
                target_force *= m_forces_on_circles[i];
                CirclesAllowed[i].my_rigidbody.AddForce(target_force);
                m_forces_on_circles[i] += Mathf.Min(MaxForce, Time.deltaTime * ForceIncreaseSpeed);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        // LOL
        int the_ball_that_touched = -1;
        for (int i = 0; i < CirclesAllowed.Length; i++) {
            if (CirclesAllowed[i].my_collider == other)
                the_ball_that_touched = i;
        }

        if (the_ball_that_touched != -1 && !m_pulling_circles[the_ball_that_touched]) {
            m_pulling_circles[the_ball_that_touched] = true;
            CirclesAllowed[the_ball_that_touched].my_rigidbody.gravityScale = 0.0f;
            CirclesAllowed[the_ball_that_touched].my_collider.enabled = false;
            CirclesAllowed[the_ball_that_touched].m_found_a_goal = true;
        }
    }

    public void StopPulling() {
        int i = 0;
        foreach (Circle c in CirclesAllowed) {
            c.m_found_a_goal = false;
            c.my_rigidbody.gravityScale = 1.0f;
            c.my_collider.enabled = true;
            m_pulling_circles[i] = false;
            m_forces_on_circles[i] = StartingForce;
            i++;
        }
    }
}
