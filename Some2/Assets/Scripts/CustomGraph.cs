using UnityEngine;
using UnityEngine.UI;

public class CustomGraph : MonoBehaviour {
    public TMPro.TMP_InputField InputField;

    private Graph m_graph;

    void Start() {
        // Get Graph component from THIS gameobject
        m_graph = GetComponent<Graph>();

        InputField.onEndEdit.AddListener(Enter);
    }

    private void Enter(string ping) {
        m_graph.SetFuction(ping);
    }
}
