using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameController : MonoBehaviour {
    [SerializeField]
    private Fader _fader = null;

    [SerializeField]
    private GameCamera _camera = null;
    public GameCamera GameCamera {
        get {
            return _camera;
        }
    }

    private WindowListener[] _windowListeners = null;
    private DamageRegion[] _damageRegions = null;
    private bool _gameOver = false;

    private void Awake() {
        _windowListeners = FindObjectsOfType<WindowListener>();
        _damageRegions = FindObjectsOfType<DamageRegion>();
    }

    public void PlayerSmashed(Vector3 position) {
        _camera.Shake(0.5f, 1f);

        foreach (WindowListener w in _windowListeners) {
            float dis = (w.transform.position - position).magnitude;
            if(dis < 6f) {
                w.NotifyHeard(dis);
            }
        }

        foreach (DamageRegion d in _damageRegions) {
            d.TryTakeDamage(position, Vector3.down);
        }
    }

    public void PlayerLose() {
        if (!_gameOver) {
            _gameOver = true;
            _fader.FadeToBlack(1f, () => {
                SceneManager.LoadScene("GameOver");
            });
        }
    }
}
