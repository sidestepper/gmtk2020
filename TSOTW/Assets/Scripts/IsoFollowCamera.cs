using UnityEngine;

public class IsoFollowCamera : MonoBehaviour
{
    [SerializeField]
    private Transform _target = null;

    [SerializeField]
    private bool _hardAttach = false;

    [SerializeField]
    private Vector3 _isoAngles = new Vector3(80f, 45f, 0f);

    [SerializeField]
    private Vector3 _fixedOffset = new Vector3(-1f, 10f, -1.5f);

    public Transform T { get; private set; }

    private float _shakeDuration = 0f;
    private float _shakeStartTime = 0f;
    private float _shakeIntensity = 0f;
    private Vector3 _centerPosition;

    private void Awake() {
        T = transform;
    }

    private void Start() {
        T.eulerAngles = _isoAngles;
    }

    public void SetHardAttach(bool hardAttach) {
        _hardAttach = hardAttach;
    }

    public void SetTarget(Transform target, bool snap) {
        _target = target;
        if (_target != null) {
            if (snap) {
                T.position = _centerPosition = _target.position + _fixedOffset;
            }
        }
    }

    public void Shake(float duration, float intensity) {
        _shakeDuration = duration;
        _shakeIntensity = intensity;
        _shakeStartTime = Time.time;
    }

    private void LateUpdate() {
        if (_target != null) {
            var targetPosition = _target.position + _fixedOffset;
            
            Vector3 nextPosition;

            if (_hardAttach) {
                nextPosition = targetPosition;
            } else {
                var between = targetPosition - T.position;
                nextPosition = _centerPosition + (between * Time.deltaTime);
            }

            _centerPosition = nextPosition;

            if (_shakeStartTime > 0f) {
                float t = Mathf.Clamp01(1.0f - ((Time.time - _shakeStartTime) / _shakeDuration));
                Vector2 offset = Random.insideUnitCircle * (_shakeIntensity * t);
                nextPosition += (T.up * offset.y) + (T.right * offset.x);
                if (t == 0f) {
                    _shakeStartTime = 0f;
                }
            }

            T.position = nextPosition;
        }
    }
}
