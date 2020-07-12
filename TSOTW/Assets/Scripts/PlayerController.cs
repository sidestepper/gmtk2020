using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayer, IAnimationEventHandler {

    public static IPlayer Current { get; private set; }

    [SerializeField]
    private Rigidbody _rigidbody = null;

    [SerializeField]
    private Transform _renderer = null;

    [SerializeField]
    private Animator _animator = null;

    [SerializeField]
    private AudioSource _smashSource = null;

    [SerializeField]
    private AudioSource _hideSource = null;

    [SerializeField]
    private GameController _gameController = null;
    public GameController GameController {
        get {
            return _gameController;
        }
    }

    private IPlayerInput _input;

    private float _xAxis = 0f;
    private float _yAxis = 0f;
    private bool _moving = false;
    private float _smashCooldown = 0f;
    private Duck _duck = null;

    public bool IsHiding { get; private set; }
    public bool IsSmashing { get; private set; }
    public bool IsDead { get; private set; }

    private void Awake() {
        Current = this;
        _input = new KeyboardPlayerInput();
        IsDead = false;
    }

    private void Update() {
        if (IsDead) {
            return;
        }

        _input.Poll(Time.deltaTime);

        if (_smashCooldown > 0f) {
            _smashCooldown -= Time.deltaTime;
        }

        if (_input.PrimaryActionDown && !IsHiding && _smashCooldown <= 0f && _duck == null) {
            // Smash
            _animator.SetTrigger("smash");
            _smashCooldown = 1f;
            IsSmashing = true;
            StopMoving();
            return;
        }

        bool hiding = _input.SecondaryActionDown && _duck == null;
        if (IsHiding && !hiding) {
            IsHiding = false;
            _animator.SetTrigger("unhide");
            _hideSource.Play();
            return;
        } else if (!IsHiding && hiding) {
            IsHiding = true;
            _animator.SetTrigger("hide");
            _hideSource.Play();
            StopMoving();
            return;
        }

        if (!IsHiding && !IsSmashing) {
            _xAxis = _input.xAxis;
            _yAxis = _input.yAxis;

            if (!Mathf.Approximately(0f, _xAxis) || !Mathf.Approximately(0f, _yAxis)) {
                if (_xAxis > 0f) {
                    _renderer.localScale = new Vector3(5f, 5f, 5f);
                } else if (_xAxis < 0f) {
                    _renderer.localScale = new Vector3(-5f, 5f, 5f);
                }

                _moving = true;
                _animator.SetBool("moving", true);
            } else if (_moving) {
                StopMoving();
            }
        }
    }

    private void StopMoving() {
        _animator.SetBool("moving", false);
        _moving = false;
        _xAxis = _yAxis = 0f;
        _rigidbody.velocity = Vector3.zero;
    }

    private void FixedUpdate() {
        if (_moving) {
            Vector3 direction = new Vector3(_xAxis, 0f, _yAxis);
            _rigidbody.velocity = direction * 3f;
        }
    }

    public void GotSpinned() {
        if (!IsDead && !IsHiding) {
            GameController.GameCamera.SetTarget(null);
            IsDead = true;
            _animator.SetBool("spinning", true);
            _rigidbody.AddForce(
                new Vector3(
                    Random.Range(1f, 3f),
                    Random.Range(-5f, 5f),
                    Random.Range(-10f, -5f)
                ),
                ForceMode.Impulse
            );

            LerpUtility.Instance.Delay(2f, () => {
                GameController.PlayerLose();
            });
        }
    }

    public void HandleAnimationEvent(string eventName) {
        if (eventName == "SmashImpact") {
            SmashImpact();
        } else if (eventName == "SmashDone") {
            SmashDone();
        }
    }

    public void SmashImpact() {
        _smashSource.Play();
        _gameController.PlayerSmashed(transform.position);
    }

    public void SmashDone() {
        IsSmashing = false;
    }

    public bool TryGetDuck(Duck duck) {
        if (IsSmashing || IsHiding || IsDead || _duck != null) {
            return false;
        }

        _animator.SetBool("grabbing", true);
        _duck = duck;
        return true;
    }

    public bool HasDuck() {
        return _duck != null;
    }

    public void ThrowDuck(Vector3 target) {
        if(_duck != null) {
            
            _duck.Throw((target - _duck.transform.position).normalized * 3f);
            _animator.SetBool("grabbing", false);
            _animator.SetTrigger("throw");

            LerpUtility.Instance.TranslateTransform(
                _duck.transform,
                _duck.transform.position,
                target,
                0.25f,
                () => {
                    _duck.Die();
                    _duck = null;
                    _gameController.PlayerStruckEngine();
                }
            );
        }
    }
}

public interface IPlayer {
    GameController GameController { get; }
    bool IsHiding { get; }
    bool IsSmashing { get; }
    bool IsDead { get; }
    void GotSpinned();
    bool TryGetDuck(Duck duck);
    bool HasDuck();
    void ThrowDuck(Vector3 target);
}