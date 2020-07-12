using UnityEngine;

public class DuckController : MonoBehaviour {
    private Duck[] _allDucks;
    private float _nextDuck = 0f;

    private void Awake() {
        _allDucks = FindObjectsOfType<Duck>();
    }

    private void Update() {
        if (Time.time >= _nextDuck) {
            _nextDuck = Time.time + Random.Range(1f, 4f);

            for (int i = 0; i < _allDucks.Length; i++) {
                if (!_allDucks[i].Spawned) {
                    _allDucks[i].Spawn(new Vector3(40f, 4f, Random.Range(-6f, 6f)), new Vector3(Random.Range(-3f, -6f), 0f, 0f));
                    break;
                }
            }
        }
    }
}
