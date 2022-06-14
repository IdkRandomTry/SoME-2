using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour {
    private LineRenderer m_line_renderer;
    private EdgeCollider2D m_edge_collider;

    [Range(0.001f, 1.0f)]
    public float Resolution;
    public float k = 1;

    private float f(float x) {
        return 1/k * Mathf.Abs(x);
    }

    void Start() {
        m_line_renderer = GetComponent<LineRenderer>();
        m_edge_collider = GetComponent<EdgeCollider2D>();

        RecalculateGraph();

        Debug.Log((new Evaluator("10.2")).Evaluate());
    }

    private void RecalculateGraph() {
        List<Vector2> points2D = new List<Vector2>();
        List<Vector3> points3D = new List<Vector3>();

        float half_width = Camera.main.orthographicSize * Camera.main.aspect;

        for (float x = -half_width; x <= half_width; x += Resolution) {
            Vector2 point2d = new Vector2(x, f(x));
            points2D.Add(point2d);
            // WTF C#... Why can I add Vector2s to a List<Vector3>. This is terrible language design if it does implicit construction
            points3D.Add(new Vector3(point2d.x, point2d.y, 0.0f));
        }

        Vector3[] v3d = points3D.ToArray();

        m_line_renderer.positionCount = v3d.Length;
        m_line_renderer.SetPositions(v3d);
        m_edge_collider.SetPoints(points2D);
    }

    void Update() { }
}
