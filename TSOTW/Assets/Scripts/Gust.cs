using UnityEngine;

public class Gust : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidBody = null;

    public bool Blowing { get; private set; }

    private Transform _transform;

    private void Awake() {
        Blowing = false;
        _transform = transform;
    }

    private void Update() {
        if(_transform.position.x < -45f) {
            Blowing = false;
        }
    }

    public void Blow(Vector3 position, Vector3 velocity) {
        if(Blowing) {
            return;
        }

        Blowing = true;
        _transform.position = position;
        _rigidBody.velocity = velocity;
    }

    public void OnTriggerEnter(Collider other) {
        if (Blowing) {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
                PlayerController.Current.GotSpinned();
            }
        }
    }
}
