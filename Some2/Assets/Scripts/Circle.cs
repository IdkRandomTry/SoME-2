using UnityEngine;

public class Circle : MonoBehaviour {
    [HideInInspector]
    public bool m_found_a_goal;

    [HideInInspector]
    public Rigidbody2D my_rigidbody;
    [HideInInspector]
    public Collider2D my_collider;

    public void Start() {
        my_rigidbody = GetComponent<Rigidbody2D>();
        my_collider = GetComponent<CircleCollider2D>();
        m_found_a_goal = false;
    }
}