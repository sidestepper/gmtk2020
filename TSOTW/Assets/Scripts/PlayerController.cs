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

    public bool IsHiding { get; private set; }
    public bool IsSmashing { get; private set; }
    public bool IsDead { get; private set; }

    private void Awake() {
        Current = this;
        _input = new KeyboardPlayerInput();
        IsDead = false;
    }

    private void Update() {
        if(IsDead) {
            return;
        }

        _input.Poll(Time.deltaTime);

        if (_input.PrimaryActionDown && !IsHiding) {
            // Smash
            _animator.SetTrigger("smash");
            IsSmashing = true;
            _moving = false;
            _xAxis = _yAxis = 0f;
            return;
        }

        bool hiding = _input.SecondaryActionDown;
        if (IsHiding && !hiding) {
            IsHiding = false;
            _animator.SetTrigger("unhide");
            return;
        } else if(!IsHiding && hiding) {
            IsHiding = true;
            _animator.SetTrigger("hide");
            _animator.SetBool("moving", false);
            _moving = false;
            _xAxis = _yAxis = 0f;
            return;
        }

        if (!IsHiding && !IsSmashing) {
            _xAxis = _input.xAxis;
            _yAxis = _input.yAxis;

            if (!Mathf.Approximately(0f, _xAxis) || !Mathf.Approximately(0f, _yAxis)) {
                if (_xAxis > 0f) {
                    _renderer.localScale = new Vector3(6f, 6f, 6f);
                } else if (_xAxis < 0f) {
                    _renderer.localScale = new Vector3(-6f, 6f, 6f);
                }

                _moving = true;
                _animator.SetBool("moving", true);
            } else if (_moving) {
                _moving = false;
                _animator.SetBool("moving", false);
            }
        }
    }

    private void FixedUpdate() {
        if (_moving) {
            Vector3 direction = new Vector3(_input.xAxis, 0f, _input.yAxis);
            _rigidbody.velocity = direction * 3f;
        }
    }

    public void Die() {
        IsDead = true;
    }

    public void HandleAnimationEvent(string eventName) {
        if (eventName == "SmashImpact") {
            SmashImpact();
        } else if(eventName == "SmashDone") {
            SmashDone();
        }
    }

    public void SmashImpact() {
        _gameController.PlayerSmashed(transform.position);
    }

    public void SmashDone() {
        IsSmashing = false;
    }
}

public interface IPlayer {
    GameController GameController { get; }
    bool IsHiding { get; }
    bool IsSmashing { get; }
    bool IsDead { get; }
}