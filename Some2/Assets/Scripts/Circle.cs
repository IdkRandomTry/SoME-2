using UnityEngine;

public class Circle : MonoBehaviour {
    [HideInInspector]
    public bool m_found_a_goal;

    [HideInInspector]
    public Rigidbody2D my_rigidbody;
    [HideInInspector]
    public Collider2D my_collider;

    [HideInInspector]
    public bool m_hit_a_side;

    public void Start() {
        my_rigidbody = GetComponent<Rigidbody2D>();
        my_collider = GetComponent<CircleCollider2D>();
        m_found_a_goal = false;
        m_hit_a_side = false;
        my_collider.enabled = true;
    }

    private bool TouchedCameraFrustum(Vector2 v) {
        float half_height = Camera.main.orthographicSize;
        float half_width = Camera.main.orthographicSize * Camera.main.aspect;
        return v.x > half_width || v.x < -half_width || v.y > half_height || v.y < -half_height;
    }


    public void Update() {
        if (TouchedCameraFrustum(my_rigidbody.position)) {
            m_hit_a_side = true;
        }
    }
}