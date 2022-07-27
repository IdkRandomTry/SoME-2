using UnityEngine;

public class AlmanacSelections : MonoBehaviour {
    private AlmanacEntry[] contents;

    void Start() {
        contents = GetComponentsInChildren<AlmanacEntry>();
    }
}
