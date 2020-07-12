using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameController : MonoBehaviour {
    [SerializeField]
    private Fader _fader = null;
    
    [SerializeField]
    private DamageUI _damageUI = null;

    [SerializeField]
    private FlightTimeUI _flightTimeUI = null;

    private bool _engineDestroyed = false;

    [SerializeField]
    private AudioSource _engineLoopSource = null;

    [SerializeField]
    private AudioSource _engineBlowupSource = null;

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

    private int _currentDamage = 0;
    private int _maxDamage = 28;
    private float _totalFlightTime = 120f;

    private void Awake() {
        _windowListeners = FindObjectsOfType<WindowListener>();
        _damageRegions = FindObjectsOfType<DamageRegion>();
        _damageUI.SetDamage(_currentDamage, _maxDamage);
        _flightTimeUI.SetFlightTime((int)_totalFlightTime);
    }

    private void Start() {
        _fader.FadeFromBlack(0.5f, null);
    }

    public void AssignDamageAssesWin() {
        _camera.transform.eulerAngles = new Vector3(_camera.transform.eulerAngles.x, _camera.transform.eulerAngles.y, _camera.transform.eulerAngles.z + (_currentDamage/4f));
        _damageUI.SetDamage(_currentDamage, _maxDamage);
        if(_currentDamage >= _maxDamage) {
            PlayerWin();
        }
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
            if(d.TryTakeDamage(position, Vector3.down)) {
                _currentDamage++;
                AssignDamageAssesWin();
            }
        }
    }

    public void PlayerStruckEngine() {
        if(_engineDestroyed) {
            return;
        }

        _engineLoopSource.Stop();
        _engineBlowupSource.Play();
        _engineDestroyed = true;
        _camera.Shake(1f, 2f);
        _currentDamage += 10;
        AssignDamageAssesWin();
        foreach (WindowListener w in _windowListeners) {
            w.NotifyHeard(0f);
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

    public void PlayerWin() {
        if (!_gameOver) {
            _gameOver = true;
            _fader.FadeToBlack(1f, () => {
                SceneManager.LoadScene("GameWin");
            });
        }
    }

    private void Update() {
        int s = (int)_totalFlightTime;
        _totalFlightTime -= Time.deltaTime;
        if ((int)_totalFlightTime < s) {
            if(_totalFlightTime <= 0f) {
                PlayerLose();
            }

            _flightTimeUI.SetFlightTime((int)_totalFlightTime);
        }
    }
}
