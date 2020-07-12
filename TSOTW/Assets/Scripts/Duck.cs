using UnityEngine;

public class Duck : MonoBehaviour {
    
    [SerializeField]
    private Rigidbody _rigidBody = null;

    [SerializeField]
    private Animator _animator = null;

    [SerializeField]
    private AudioSource _audio = null;
    private float _nextQuackTime = 0f;

    public bool Spawned { get; private set; }
    public bool Grabbed { get; private set; }
    public bool Thrown { get; private set; }

    private Transform _transform;

    private void Awake() {
        Spawned = false;
        _transform = transform;
    }

    private void Update() {
        if (_transform.position.x < -45f) {
            Spawned = false;
        } else if(Spawned) {
            if(Time.time >= _nextQuackTime) {
                _nextQuackTime = Time.time + Random.Range(5f, 10f);
                _audio.Play();
            }
        }
    }

    public void Spawn(Vector3 position, Vector3 velocity) {
        if (Spawned) {
            return;
        }

        Spawned = true;
        _transform.position = position;
        _rigidBody.velocity = velocity;
        _rigidBody.isKinematic = false;
        Grabbed = Thrown = false;
        _animator.SetBool("grabbed", false);
        _animator.SetBool("thrown", false);
    }

    public void Grab(Transform newParent) {
        if (!Grabbed) {
            Grabbed = true;
            Thrown = false;
            _animator.SetBool("grabbed", true);
            _animator.SetBool("thrown", false);
            _transform.parent = newParent;
            _rigidBody.velocity = Vector3.zero;
            //.25 when facing right
            _transform.localPosition = new Vector3(-0.382f, 1.381f, 0f);
            _rigidBody.isKinematic = true;
            
        }
    }

    public void Throw(Vector3 velocity) {
        if (!Thrown) {
            _transform.parent = null;
            Grabbed = false;
            Thrown = true;
            _animator.SetBool("grabbed", false);
            _animator.SetBool("thrown", true);
        }
    }

    public void Die() {
        Spawned = false;
        _animator.SetBool("grabbed", false);
        _animator.SetBool("thrown", false);
        Thrown = false;
    }

    public void OnTriggerEnter(Collider other) {
        if (Spawned) {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
                if(PlayerController.Current.TryGetDuck(this)) {
                    Grab(other.transform);
                }
            }
        }
    }
}
