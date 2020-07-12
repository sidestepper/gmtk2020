using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ClickPlay() {
        SceneManager.LoadScene("GameScene");
    }

    public void ClickHowToPlay() {
        SceneManager.LoadScene("HowtoPlay");
    }

    public void ClickOK() {
        SceneManager.LoadScene("MainMenu");
    }

    public void ClickQuit() {
        Application.Quit();
    }
}
