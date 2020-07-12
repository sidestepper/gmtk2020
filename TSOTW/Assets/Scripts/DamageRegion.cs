using UnityEngine;

public class DamageRegion : MonoBehaviour {
    
    [SerializeField]
    private MeshRenderer[] _damageDecals = null;

    [SerializeField]
    private Collider _collider = null;

    private int _totalDamage = 0;

    public bool IsDestroyed {
        get {
            return _totalDamage >= _damageDecals.Length;
        }
    }

    private void Awake() {
        foreach (MeshRenderer r in _damageDecals) {
            r.enabled = false;
        }
    }

    public bool TryTakeDamage(Vector3 origin, Vector3 direction) {
        RaycastHit rchit;
        if (Physics.Raycast(origin, direction, out rchit, Mathf.Infinity, 1 << LayerMask.NameToLayer("DamageRegion"))) {
            if (rchit.collider == _collider) {
                TakeDamage();
                return true;
            }
        }

        return false;
    }

    public void TakeDamage() {
        _totalDamage = Mathf.Clamp(_totalDamage + 1, 0, _damageDecals.Length);
        for(int i = 0; i < _totalDamage; i++) {
            _damageDecals[i].enabled = true;
        }
    }
}
