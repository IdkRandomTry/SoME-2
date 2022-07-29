using UnityEngine;

public class AlmanacNextPageButton : MonoBehaviour {
    public Animator In;
    public Animator Out;
    public Almanac AlmanacContext;
    public int NextPageNum;

    public void OnClick() {
        if (AlmanacContext.m_currently_presenting_page != NextPageNum) {
            In.SetTrigger("In");
            Out.SetTrigger("Out");
            AlmanacContext.m_currently_presenting_page = NextPageNum;
        }
    }
}
