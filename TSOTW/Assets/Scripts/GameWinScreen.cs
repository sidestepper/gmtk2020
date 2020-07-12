using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWinScreen : MonoBehaviour {
    private void Start() {
        LerpUtility.Instance.Delay(2f, () => {
            SceneManager.LoadScene("MainMenu");
        });
    }
}
