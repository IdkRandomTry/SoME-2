using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour {
    private LineRenderer m_line_renderer;
    private int m_current_collider_index = 0;
    private List<EdgeCollider2D> m_edge_colliders = new List<EdgeCollider2D>();

    private ASTNode m_previous_working_ast;
    private ASTNode m_target_function;
    private bool m_start_tinterp;
    private Evaluator m_evaluator;

    private float t = 0.0f;

    [Range(0.001f, 1.0f)]
    public float Resolution;
    public string SourceCode;
    public float WidthOverride = 0.1f;

    private float f(float x) {
        return Mathf.Lerp(
            m_evaluator.Evaluate(m_previous_working_ast, x),
            m_evaluator.Evaluate(m_target_function, x),
            t);
    }

    void Start() {
        if (!m_line_renderer) m_line_renderer = GetComponent<LineRenderer>();
        // Remove all edge colliders
        m_current_collider_index = 0;
        m_edge_colliders.Clear();
        m_edge_colliders.Add(GetComponent<EdgeCollider2D>());

        m_line_renderer.widthCurve = new AnimationCurve(new Keyframe(0, WidthOverride), new Keyframe(1, WidthOverride));

        m_evaluator = new Evaluator(SourceCode);
        m_previous_working_ast = m_evaluator.Parse();
        m_target_function = m_previous_working_ast;

        RecalculateGraph();
    }

    private const float tolerance = 1.0f;

    private bool IsInCameraBounds(Vector2 v) {
        float half_height = Camera.main.orthographicSize + tolerance;
        float half_width = Camera.main.orthographicSize * Camera.main.aspect + tolerance;
        return v.x >= -half_width && v.x <= half_width && v.y >= -half_height && v.y <= half_height;
    }

    private bool IsBelowCam(Vector2 v) {
        float half_height = Camera.main.orthographicSize + tolerance;
        return v.y == -half_height;
    }

    private bool IsAboveCam(Vector2 v) {
        float half_height = Camera.main.orthographicSize + tolerance;
        return v.y == half_height;
    }

    private void ClampToCam(ref Vector2 v) {
        float half_height = Camera.main.orthographicSize + tolerance;
        float half_width = Camera.main.orthographicSize * Camera.main.aspect + tolerance;
        v.x = Mathf.Clamp(v.x, -half_width, half_width);
        v.y = Mathf.Clamp(v.y, -half_height, half_height);
    }

    private void NextEdgeCollider() {
        m_current_collider_index++;
        if (m_current_collider_index == m_edge_colliders.Count) {
            EdgeCollider2D new_collider = gameObject.AddComponent<EdgeCollider2D>();
            new_collider.offset = m_edge_colliders[0].offset;
            m_edge_colliders.Add(new_collider);
        } else {
            m_edge_colliders[m_current_collider_index].enabled = true;
        }
    }

    private void RecalculateGraph() {
        m_current_collider_index = 0;

        List<Vector2> points2D = new List<Vector2>();
        List<Vector3> points3D = new List<Vector3>();

        float half_width = Camera.main.orthographicSize * Camera.main.aspect;

        Vector2 last;
        Vector2 curr = new Vector2(-half_width - Resolution, f(-half_width - Resolution));
        Vector2 next = new Vector2(-half_width - Resolution, f(-half_width - Resolution));
        for (float x = -half_width - Resolution; x <= half_width + Resolution; x += Resolution) {
            last = curr;
            curr = next;
            next = new Vector2(x, f(x));

            if (!(IsInCameraBounds(last) && IsInCameraBounds(next) && IsInCameraBounds(curr))) {
                ClampToCam(ref curr);
            }

            if ((IsAboveCam(last) && IsBelowCam(curr)) || (IsBelowCam(last) && IsAboveCam(curr))) {
                m_edge_colliders[m_current_collider_index].SetPoints(points2D);
                NextEdgeCollider();
                points2D.Clear();

                points3D.Add(new Vector3(last.x, last.y, -360));
                points3D.Add(new Vector3(curr.x, curr.y, -360));
            }

            if (float.IsNaN(curr.y)) {
                continue;
            }

            points2D.Add(curr);
            points3D.Add(curr);
        }

        Vector3[] v3d = points3D.ToArray();

        m_line_renderer.positionCount = v3d.Length;
        m_line_renderer.SetPositions(v3d);
        m_edge_colliders[m_current_collider_index].SetPoints(points2D);

        for (int i = m_current_collider_index + 1; i < m_edge_colliders.Count; i++)
            m_edge_colliders[i].enabled = false;
    }

    void Update() {
        if (m_target_function != m_previous_working_ast) {
            if (t >= 1) {
                StopCoroutine("LerpT");
                t = 0.0f;
                m_previous_working_ast = m_target_function;
            }
            RecalculateGraph();
        }
    }

    public IEnumerator LerpT() {
        // Configurable resolution here too?
        float step = 0.8f;
        for (float x = 0; x <= 1 + step; x += step * Time.deltaTime) {
            // Configurable Easing function here?
            // Cubic Ease In-Out from easings.net
            t = (x < 0.5 ? 4 * x*x*x : 1 - ((-2 * x + 2)*(-2 * x + 2)*(-2 * x + 2)) / 2);
            yield return null;
        }
    }

    public void SetFuction(string code, bool should_change) {
        StartCoroutine("LerpT");
        t = 0.0f;
        SourceCode = code;
        m_evaluator.Reset(SourceCode);
        ASTNode node = m_evaluator.Parse();
        if (m_evaluator.errored) {
            return;
        }
        m_evaluator.Dump(node);
        if (should_change) {
            m_target_function = node;
            RecalculateGraph();
        }
    }
}
