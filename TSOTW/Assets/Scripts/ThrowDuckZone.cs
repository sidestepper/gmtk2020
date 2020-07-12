using UnityEngine;

public class ThrowDuckZone : MonoBehaviour {
    
    [SerializeField]
    private Transform _throwDestination = null;

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            if (PlayerController.Current.HasDuck()) {
                PlayerController.Current.ThrowDuck(_throwDestination.position);
            }
        }
    }
}
