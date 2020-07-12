using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [SerializeField]
    private Transform _target = null;

    [SerializeField]
    private Vector3 _offset = Vector3.zero;

    [SerializeField]
    private Vector3 _angles = Vector3.zero;

    private Transform _transform;
    private float _shakeDuration = 0f;
    private float _shakeStartTime = 0f;
    private float _shakeIntensity = 0f;
    private Vector3 _centerPosition;

    private void Awake() {
        _transform = transform;
    }

    private void Start() {
        _transform.eulerAngles = _angles;
    }

    public void Shake(float duration, float intensity) {
        _shakeDuration = duration;
        _shakeIntensity = intensity;
        _shakeStartTime = Time.time;
    }

    private void LateUpdate() {
        if (_target != null) {
            var targetPosition = _target.position + _offset;
            Vector3 nextPosition;
            nextPosition = targetPosition;
            _centerPosition = nextPosition;

            if (_shakeStartTime > 0f) {
                float t = Mathf.Clamp01(1.0f - ((Time.time - _shakeStartTime) / _shakeDuration));
                Vector2 offset = Random.insideUnitCircle * (_shakeIntensity * t);
                nextPosition += (_transform.up * offset.y) + (_transform.right * offset.x);
                if (t == 0f) {
                    _shakeStartTime = 0f;
                }
            }

            _transform.position = nextPosition;
        }
    }
}
