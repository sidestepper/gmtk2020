using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    private void Start() {
        LerpUtility.Instance.Delay(2f, () => {
            SceneManager.LoadScene("GameScene");
        });
    }
}
