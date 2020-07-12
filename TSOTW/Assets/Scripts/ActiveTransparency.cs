using UnityEngine;
using System.Collections.Generic;

public class ActiveTransparency : MonoBehaviour {
    
    [SerializeField]
    private Transform _target = null;

    private Transform _transform;

    private Dictionary<Collider, TransparencyHandler> _transObjects = new Dictionary<Collider, TransparencyHandler>();

    private void Awake() {
        _transform = transform;
        TransparencyHandler[] handlers = FindObjectsOfType<TransparencyHandler>();
        foreach(TransparencyHandler t in handlers) {
            _transObjects.Add(t.Collider, t);
        }
    }

    private void LateUpdate() {
        Vector3 d = _target.position - _transform.position;

        var e = _transObjects.GetEnumerator();
        while(e.MoveNext()) {
            e.Current.Value.GoTransparent(false);
        }

        RaycastHit rchit;
        if(Physics.Raycast(_transform.position, d, out rchit, Mathf.Infinity, 1 << LayerMask.NameToLayer("ActiveTransparent"))) {
            if(_transObjects.ContainsKey(rchit.collider)) {
                _transObjects[rchit.collider].GoTransparent(true);
            }
        }
    }
}