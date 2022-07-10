using UnityEngine;

public class Goal : MonoBehaviour {
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
                Debug.Log("Test");
                m_pulling = true;
            }
        }
    }
}
