using UnityEngine;
using UnityEngine.UI;
using System;

public class Fader : MonoBehaviour {
    
    [SerializeField]
    private Image _fadeOverlay = null;

    private bool _fading = false;
    private float _start = 0f;
    private float _duration = 0f;
    private Color _from;
    private Color _to;
    private Action _callback = null;

    private void Awake() {
        _fadeOverlay.enabled = false;
    }

    public void FadeToBlack(float duration, Action callback) {
        Fade(
            new Color(0f, 0f, 0f, 0f),
            new Color(0f, 0f, 0f, 1f),
            duration,
            callback
        );
    }

    public void FadeFromBlack(float duration, Action callback) {
        Fade(
            new Color(0f, 0f, 0f, 1f),
            new Color(0f, 0f, 0f, 0f),
            duration,
            callback
        );
    }

    public void Fade(Color from, Color to, float duration, Action callback) {
        _fadeOverlay.color = from;
        _fadeOverlay.enabled = true;

        _from = from;
        _to = to;

        _start = Time.time;
        _duration = duration;
        
        _callback = callback;

        _fading = true;
    }

    private void Update() {
        if(_fading) {
            float t = Mathf.Clamp01((Time.time - _start) / _duration);
            _fadeOverlay.color = Color.Lerp(_from, _to, t);
            if(t >= 1f) {
                _fading = false;
                _callback?.Invoke();
            }
        }
    }
}
