using UnityEngine;

public class TransparencyHandler : MonoBehaviour {
    [SerializeField]
    private Renderer _renderer = null;

    [SerializeField]
    private Collider _collider = null;

    public Collider Collider {
        get {
            return _collider;
        }
    }

    public void GoTransparent(bool goTransparent) {
        if (goTransparent) {
            _renderer.material.color = new Color(1f, 1f, 1f, 0.3f);
        } else {
            _renderer.material.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
