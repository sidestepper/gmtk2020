using UnityEngine;

public class GustController : MonoBehaviour
{
    private Gust[] _allGusts;
    private float _nextGust = 0f;

    private void Awake() {
        _allGusts = FindObjectsOfType<Gust>();
    }

    private void Update() {
        if(Time.time >= _nextGust) {
            //_nextGust = Time.time + Random.Range(0.1f, 1f);
            _nextGust = Time.time + Random.Range(1f, 4f);

            for (int i = 0; i < _allGusts.Length; i++) {
                if(!_allGusts[i].Blowing) {
                    _allGusts[i].Blow(new Vector3(40f, 4f, Random.Range(-6f, 6f)), new Vector3(Random.Range(-6f, -10f), 0f, 0f));
                    break;
                }
            }
        }
    }
}
