using UnityEngine;

public class Almanac : MonoBehaviour {
    public Transition transition;

    [HideInInspector]
    public int m_currently_presenting_page;

    void Start() {
        m_currently_presenting_page = 0;
    }
    
    public void OnBackButtonClick() {
        transition.SwitchSceneTo("MainMenu");
    }
}
