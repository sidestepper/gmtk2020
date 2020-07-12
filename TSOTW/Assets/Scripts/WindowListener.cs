using UnityEngine;

public class WindowListener : MonoBehaviour {
    
    [SerializeField]
    private GameObject _renderer = null;

    private MeshRenderer _r;
    private int _currentTransformation = -1;

    private bool _opening = false;
    private bool _opened = false;

    private void Awake() {
        _r = _renderer.GetComponent<MeshRenderer>();
        _r.material.color = Color.green;
    }

    public void NotifyHeard(float distance) {
        if(_opening) {
            return;
        }

        if(Random.Range(0, Mathf.CeilToInt(distance)) == 0) {
            _opening = true;
            _r.material.color = Color.yellow;
            _currentTransformation = LerpUtility.Instance.Delay(1f, () => {
                _r.material.color = Color.red;
                _opened = true;
                _currentTransformation = LerpUtility.Instance.Delay(3f, () => {
                    _opening = _opened = false;
                    _r.material.color = Color.green;
                });
            });
        }
    }

    private void Update() {
        if(_opened) {
            if (!PlayerController.Current.IsHiding) {
                PlayerController.Current.GameController.PlayerLose();
            }
        }
    }
}
