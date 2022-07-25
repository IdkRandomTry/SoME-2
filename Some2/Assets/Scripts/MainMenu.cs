using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public void OnStartButtonClick() {
        SceneManager.LoadScene("Level01");
    }

    public void OnAlmanacButtonClick() {
        Debug.Log("Almanac Open");
    }

    public void OnLevelSelectButtonClick() {
        SceneManager.LoadScene("LevelSelect");
    }

    public void OnQuitButtonClick() {
        Application.Quit(0);
    }
}
