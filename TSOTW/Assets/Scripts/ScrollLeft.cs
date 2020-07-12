using UnityEngine;

public class ScrollLeft : MonoBehaviour
{
    [SerializeField]
    private float _minX = 0f;

    [SerializeField]
    private float _speed = 0f;

    [SerializeField]
    private float _scrollOffsetX = 10f;

    private Transform _transform;

    private void Awake() {
        _transform = transform;
    }

    private void Update() {
        float nextX = _transform.localPosition.x - (_speed * Time.deltaTime);
        if(nextX < _minX) {
            nextX += _scrollOffsetX;
        }

        _transform.localPosition = new Vector3(
            nextX,
            _transform.localPosition.y,
            _transform.localPosition.z
        );
    }
}
