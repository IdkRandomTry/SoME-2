using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour {
    public void OnBackButtonClick() {
        SceneManager.LoadScene("MainMenu");
    }
}
