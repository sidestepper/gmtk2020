using UnityEngine;
using System;
using System.Collections.Generic;

public sealed class LerpUtility : MonoBehaviour {

    private static int _nextID = 1;

    private abstract class TransformOverTime {
        public int ID { get; protected set;  }
        public abstract bool Update(float time, float deltaTime);
        public bool Canceled;
    }

    private sealed class Rotation : TransformOverTime {
        private readonly Transform _transform;
        private readonly float _degreesPerSecond;
        private readonly Quaternion _targetRotation;
        private readonly Vector3 _targetAngles;
        private readonly Action _callback;

        public Rotation(Transform transform, float degreesPerSecond, Quaternion targetRotation, Vector3 targetAngles, Action callback) {
            ID = _nextID++;
            _transform = transform;
            _degreesPerSecond = degreesPerSecond;
            _targetRotation = targetRotation;
            _targetAngles = targetAngles;
            _callback = callback;
        }

        public override bool Update(float time, float deltaTime) {
            if(Canceled) {
                return false;
            }

            _transform.rotation = Quaternion.RotateTowards(_transform.rotation, _targetRotation, _degreesPerSecond * deltaTime);
            float angle = Vector3.Angle(_transform.forward, _targetAngles);
            if (angle < 1f) {
                _transform.rotation = _targetRotation;
                _callback?.Invoke();
                return false;
            }

            return true;
        }
    }

    private sealed class Lerp3D : TransformOverTime {
        private readonly Transform _transform;
        private readonly float _startTime;
        private readonly float _duration;
        private readonly Vector3 _from;
        private readonly Vector3 _to;
        private readonly Action _callback;

        public Lerp3D(Transform transform, float startTime, float duration, Vector3 from, Vector3 to, Action callback) {
            ID = _nextID++;
            _transform = transform;
            _startTime = startTime;
            _duration = duration;
            _from = from;
            _to = to;
            _callback = callback;
        }

        public override bool Update(float time, float deltaTime) {
            if(_transform == null) {
                return false;
            }

            if (Canceled) {
                return false;
            }

            float t = Mathf.Clamp01((time - _startTime) / _duration);
            if (t == 1f) {
                _transform.localPosition = _to;
                _callback?.Invoke();
                return false;
            }
            
            _transform.localPosition = _from + ((_to - _from) * t);
            return true;
        }
    }

    private sealed class Timer : TransformOverTime {

        private float _delay;
        private Action _callback;

        public Timer(float delay, Action callback) {
            ID = _nextID++;
            _delay = delay;
            _callback = callback;
        }

        public override bool Update(float time, float deltaTime) {
            if (Canceled) {
                return false;
            }

            _delay -= deltaTime;
            if(_delay <= 0f) {
                _callback?.Invoke();
                return false;
            }

            return true;
        }
    }

    public static LerpUtility Instance { get; private set; }

    private List<TransformOverTime> _transformations = new List<TransformOverTime>();
    private Queue<TransformOverTime> _transformationsToRemove = new Queue<TransformOverTime>();
    private float _currentTime;

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        float dt = Time.deltaTime;
        _currentTime = Time.time;
        
        for(int i = 0; i < _transformations.Count; i++) {
            TransformOverTime transformation = _transformations[i];
            if (!transformation.Update(_currentTime, dt)) {
                _transformationsToRemove.Enqueue(transformation);
            }
        }

        while(_transformationsToRemove.Count > 0) {
            _transformations.Remove(_transformationsToRemove.Dequeue());
        }
    }

    public void CancelAll() {
        _transformations.Clear();
    }

    public void Cancel(int ID) {
        for(int i = 0; i < _transformations.Count; i++) {
            TransformOverTime t = _transformations[i];
            if(t.ID == ID) {
                t.Canceled = true;
                return;
            }
        }
    }

    public int ActorTurn(Transform transform, float degreesPerSecond, Vector3 targetForward, Quaternion rotation, Action callback) {
        Rotation r = new Rotation(transform, degreesPerSecond, rotation, targetForward, callback);
        _transformations.Add(r);
        return r.ID;
    }

    public int ActorStep(Transform transform, int fromX, int fromY, int fromAltitude, int toX, int toY, int toAltitude, float stepDuration, Action callback) {
        return TranslateTransform(transform, new Vector3(fromX, fromAltitude / 2f, fromY), new Vector3(toX, toAltitude / 2f, toY), stepDuration, callback);
    }

    public int TranslateTransform(Transform transform, Vector3 from, Vector3 to, float duration, Action callback) {
        Lerp3D lerp = new Lerp3D(
            transform,
            _currentTime,
            duration,
            from,
            to,
            callback
        );

        _transformations.Add(lerp);
        return lerp.ID;
    }

    public int Delay(float delay, Action callback) {
        Timer timer = new Timer(delay, callback);
        _transformations.Add(timer);
        return timer.ID;
    }
}
