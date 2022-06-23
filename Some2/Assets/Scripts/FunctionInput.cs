using UnityEngine.EventSystems;

public class FunctionInput : UIBehaviour {
    private string m_newly_set;

    // {} -> Editable Portion
    // nothing -> Static Portion
    // USES CHILDREN IN ORDER
    // Would look something like an array of "{}" "* x^2 +" "{}" "* x +" "{}"... yeah
    public string[] Format;
    
    private TMPro.TMP_InputField[] m_input_fields;
    private TMPro.TMP_Text[] m_texts;

    public Graph AssociatedGraph;
    public bool ShouldChangeTheGraph;

    protected override void Start() {
        base.Start();

        m_input_fields = GetComponentsInChildren<TMPro.TMP_InputField>();
        m_texts = GetComponentsInChildren<TMPro.TMP_Text>();

        foreach (TMPro.TMP_InputField field in m_input_fields) {
            field.onEndEdit.AddListener(StringPing);
        }
    }

    // Nice name
    public void StringPing(string _) {
        m_newly_set = "";
        int input_field_index = 0;

        // Recombine all strings
        foreach (string c in Format) {
            if (c == "{}") {
                m_newly_set += m_input_fields[input_field_index++].text;
            } else {
                m_newly_set += c;
            }
        }

        if (AssociatedGraph != null)
            AssociatedGraph.SetFuction(m_newly_set, ShouldChangeTheGraph);
    }
}
