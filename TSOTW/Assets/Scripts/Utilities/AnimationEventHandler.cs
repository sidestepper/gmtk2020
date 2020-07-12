using UnityEngine;

public interface IAnimationEventHandler {
    void HandleAnimationEvent(string eventName);
}

public class AnimationEventHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject _targetObject = null;

    private IAnimationEventHandler _target;

    private void Awake() {
        _target = _targetObject?.GetComponent<IAnimationEventHandler>();
    }

    public void HandleAnimationEvent(string eventName) {
        _target?.HandleAnimationEvent(eventName);
    }
}
